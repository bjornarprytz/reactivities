import { createContext, useContext } from "react"
import ReactivityStore from "./activityStore"

interface Store {
    activityStore: ReactivityStore
}

export const store: Store = {
    activityStore: new ReactivityStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}