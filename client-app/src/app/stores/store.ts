import { createContext, useContext } from "react"
import ReactivityStore from "./activityStore"
import CommonStore from "./commonStore";

interface Store {
    activityStore: ReactivityStore;
    commonStore: CommonStore;
}

export const store: Store = {
    activityStore: new ReactivityStore(),
    commonStore: new CommonStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}