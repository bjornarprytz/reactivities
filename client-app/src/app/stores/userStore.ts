import { makeAutoObservable } from "mobx";
import { history } from "../../features/history/history";
import { User, UserFormValues } from "../../features/users/user";
import agent from "../api/agent";
import { store } from "./store";

export default class UserStore {
    user: User | null = null;
    
    constructor(){
        makeAutoObservable(this);
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (creds:UserFormValues) => {
        try {
            const user = await agent.Account.login(creds)
            store.commonStore.setToken(user.token);
            this.setUser(user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        this.setUser(null);
        history.push('/');
    }

    register = async (creds : UserFormValues) => {
        try {
            const user = await agent.Account.register(creds)
            store.commonStore.setToken(user.token);
            this.setUser(user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    } 

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            this.setUser(user);
        } catch (error) {
            console.log(error);
        }
    }

    setImage = (image: string) => {
        if (this.user) this.user.image = image;
    }

    setDisplayName = (name: string) => {
        if (this.user) this.user.displayName = name;
    }

    private setUser = (user:User | null) => {
        this.user = user;
    }
}