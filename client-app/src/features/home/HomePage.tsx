import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { Container, Header, Segment, Image, Button } from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import LoginForm from '../users/LoginForm';
import RegisterForm from '../users/RegisterForm';

export default observer(function HomePage() {
    const {
        userStore: {isLoggedIn},
        modalStore: {openModal}
    } = useStore();

    return (
        <Segment inverted textAlign='center' vertical className='masthead'>
            <Container text>
                <Header as='h1' inverted>
                    <Image size='massive' src='/assets/logo.png' alt='logo' />
                    Reactivities
                </Header>
                {isLoggedIn ? (
                    <>
                        <Header as='h2' inverted content='Welcome to Reactivities' />
                        <Button as={Link} to='/activities' size='huge' inverted content="Go to activities!" />
                    </>
                ) : (
                    <>
                        <Button onClick={() => openModal(<LoginForm />)} size='huge' inverted>
                            Login
                        </Button>
                        <Button onClick={() => openModal(<RegisterForm />)} size='huge' inverted>
                            Register
                        </Button>

                    </>
                )}
            </Container>
        </Segment>
    )
})