import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroller';
import { Grid, Loader } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponents';
import { PagingParams } from '../../../app/models/pagination';
import { useStore } from '../../../app/stores/store';
import ReactivityFilters from './ReactivityFilters';
import ReactivityList from './ReactivityList';
import ReactivityListItemPlaceholder from './ReactivityLIstItemPlaceholder';

export default observer(function ReactivityDashboard() {
    const {activityStore : {loadActivities, activityRegistry, loadingInitial, setPagingParams, pagination}} = useStore();

    const [loadingNext, setLoadingNext] = useState(false);

    function handleGetNext(){
        setLoadingNext(true);
        setPagingParams(new PagingParams(pagination!.currentPage + 1));
        loadActivities().then(() => setLoadingNext(false));
    }

    useEffect(() => {
        if (activityRegistry.size <= 1) loadActivities();
    }, [loadActivities, activityRegistry.size]);

    return (
        <Grid>
            <Grid.Column width='10'>
                {loadingInitial && !loadingNext ? (
                    <>
                        <ReactivityListItemPlaceholder />
                        <ReactivityListItemPlaceholder />
                    </>
                ) : (
                    <InfiniteScroll 
                        pageStart={0}
                        loadMore={handleGetNext}
                        hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                        initialLoad={false}
                    >
                        <ReactivityList />
                    </InfiniteScroll>
                )}
            </Grid.Column>
            <Grid.Column width='6'>
                <ReactivityFilters />
            </Grid.Column>
            <Grid.Column width={10}>
                <Loader active={loadingNext} />
            </Grid.Column>
        </Grid>
    )
})