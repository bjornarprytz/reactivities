import { observer } from "mobx-react-lite";
import { Modal } from "semantic-ui-react";
import { useStore } from "../../stores/store";


export default observer(function ModalContainer() {
    const {modalStore : {modal, closeModal}} = useStore();

    return (
        <Modal open={modal.open} onClose={closeModal} size='mini'>
            <Modal.Content>
                {modal.body}
            </Modal.Content>
        </Modal>
    )
})