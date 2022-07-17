import { ErrorMessage, Form, Formik } from "formik"
import { Button, Header } from "semantic-ui-react"
import MyTextInput from "../../app/common/form/MyTextInput"
import ValidationErrors from "../errors/ValidationErrors"
import * as Yup from 'yup';
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import MyTextArea from "../../app/common/form/MyTextArea";

interface Props {
    setEditMode: (state: boolean) => void;
}

export default observer(function ProfileForm({setEditMode}: Props) {
    const {profileStore} = useStore();
    const {profile} = profileStore;

    const validationSchema = Yup.object({
        displayName: Yup.string().required(),
    })


    return (
        <Formik
            initialValues={{displayName: profile?.displayName, bio: profile?.bio}}
            onSubmit={(values) => profileStore.updateProfile(values).then(() => setEditMode(false))}
            validationSchema={validationSchema}
        >
            {({isSubmitting, isValid, dirty}) => (
                <Form className='ui form error' autoComplete='off'>
                    <MyTextInput name='displayName' placeholder='Display Name' />
                    <MyTextArea name='bio' placeholder='Add your bio' rows={3} />
                    <Button disabled={!isValid || !dirty} loading={isSubmitting} positive content='Update profile' type='submit' fluid floated='right' />
                </Form>
            )}
        </Formik>
    )
})