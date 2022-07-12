import { format } from "date-fns";
import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Reactivity } from "../models/reactivity";
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

    createActivity = async (activity: Reactivity) => {
        this.setLoading(true);

        try {
            await agent.Activities.create(activity);
            runInAction(() => {
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.setLoading(false);
            })
        } catch (error) {
            console.log(error);
            this.setLoading(false);
        }
    }

    updateActivity = async (activity: Reactivity) => {
        this.setLoading(true);
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.setLoading(false);
            })
        } catch (error) {
            console.log(error);
            this.setLoading(false);
        }
    }

    deleteActivity = async (id: string) => {
        this.setLoading(true);
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activityRegistry.delete(id);
                this.setLoading(false);
            })
        } catch (error) {
            console.log(error);
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
