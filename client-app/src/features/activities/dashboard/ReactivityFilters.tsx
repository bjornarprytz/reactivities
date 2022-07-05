import { observer } from 'mobx-react-lite';
import React from 'react';
import Calendar from 'react-calendar';
import { Header, Menu } from 'semantic-ui-react';


export default observer(function ReactivityFilters() {
    return (
        <>
            <Menu vertical size='large' style={{width: '100%', marginTop: 30}}>
                <Header icon='filter' attached color='teal' contents='Filters' />
                <Menu.Item content='All Activities' />
                <Menu.Item content="I'm going" />
                <Menu.Item content="I'm hosting" />
            </Menu>
            <Header />
            <Calendar />
        </>
    )
})