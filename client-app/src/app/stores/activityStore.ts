import { format } from "date-fns";
import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Profile } from "../models/profile";
import { Reactivity, ReactivityFormValues } from "../models/reactivity";
import { store } from "./store";

export default class ReactivityStore {
    activityRegistry = new Map<string, Reactivity>();
    selectedActivity: Reactivity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = true;

    constructor(){
        makeAutoObservable(this);
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => 
            a.date!.getTime() - b.date!.getTime());
    }

    get groupedActivities() {
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                const date = format(activity.date!, 'dd MM yyyy');
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];

                return activities;
            }, {} as {[key: string]: Reactivity[]})
        )
    }

    loadActivities = async () => {
        this.setLoadingInitial(true);
        
        try {
            const activities = await agent.Activities.list();
            
            runInAction(() => {
                activities.forEach(a => {
                    this.insertActivity(a);
                })
            })
            this.setLoadingInitial(false);
        } catch (error){
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {
        const activityFromMemory = this.getActivity(id);

        if (activityFromMemory) {
            this.selectedActivity = activityFromMemory;
            return activityFromMemory;
        } else {
            this.setLoadingInitial(true);

            try {
                const activityFromBackend = await agent.Activities.details(id);
                
                this.insertActivity(activityFromBackend);
                this.setSelectedActivity(activityFromBackend);
                this.setLoadingInitial(false);
                return activityFromBackend;
            } catch(error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    setSelectedActivity = (activity: Reactivity) => {
        this.selectedActivity = activity;
    }

    createActivity = async (activity: ReactivityFormValues) => {
        const user = store.userStore.user;
        const attendee = new Profile(user!);

        try {
            await agent.Activities.create(activity);
            const newActivity = new Reactivity(activity);
            newActivity.hostUsername = user!.username;
            newActivity.attendees =  [attendee];

            this.insertActivity(newActivity);

            runInAction(() => {
                this.selectedActivity = newActivity;
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateActivity = async (activity: ReactivityFormValues) => {
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                if (activity.id) {
                    let updatedActivity = {...this.getActivity(activity.id), ...activity} as Reactivity;
                    this.activityRegistry.set(activity.id, updatedActivity);
                    this.selectedActivity = updatedActivity;
                }
            })
        } catch (error) {
            console.log(error);
        }
    }

    deleteActivity = async (id: string) => {
        this.setLoading(true);
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activityRegistry.delete(id);
            })
        } catch (error) {
            console.log(error);
        } finally {
            this.setLoading(false);
        }
    }

    updateAttendance = async () => {
        const user = store.userStore.user;
        this.loading = true;
        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                if (this.selectedActivity?.isGoing) {
                    this.selectedActivity.attendees = 
                        this.selectedActivity.attendees?.filter(a => a.username !== user?.username);
                    
                    this.selectedActivity.isGoing = false;
                } else {
                    const attendee = new Profile(user!);

                    this.selectedActivity?.attendees?.push(attendee);
                    this.selectedActivity!.isGoing = true;
                }

                this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        } catch {

        } finally {
            this.setLoading(false);
        }
    }

    cancelActivityToggle =async () => {
        this.loading = true;

        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                this.selectedActivity!.isCancelled = !this.selectedActivity?.isCancelled;
                this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        } catch (error) {
            console.log(error);
        } finally {
            this.setLoading(false);
        }

    }

    private insertActivity(activity: Reactivity) {
        const user = store.userStore.user;
        if (user) {
            activity.isGoing = activity.attendees!.some(
                a => a.username === user.username
            )
            activity.isHost = activity.hostUsername === user.username;
            activity.host = activity.attendees?.find(x => x.username === activity.hostUsername);
        }

        activity.date = new Date(activity.date!);
        this.activityRegistry.set(activity.id, activity);
    }

    private getActivity(id: string) {
        return this.activityRegistry.get(id);
    }
}
