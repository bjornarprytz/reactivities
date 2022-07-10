import { observer } from 'mobx-react-lite';
import React from 'react';
import { Message } from 'semantic-ui-react';

interface Props {
    errors: any;
}

export default observer(function ValidationErrors({errors}: Props){
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: any, i: any) => (
                        <Message.Item key={i}>
                            {err}
                        </Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
    )
})