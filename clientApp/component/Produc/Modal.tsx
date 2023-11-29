import React, {FC, ReactElement} from 'react';
import {Modal} from "@material-ui/core";

const ProductModal: FC<Props> = ({handleClose, open, children}) => {
    return <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="simple-modal-title"
        aria-describedby="simple-modal-description"
    >
        {children}
    </Modal>
};
type Props = {
    handleClose: () => void
    open: boolean
    children: ReactElement
}
export default ProductModal