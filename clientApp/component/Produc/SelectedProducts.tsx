import React, {FC, useState} from 'react';
import ProductModal from "./Modal";

const SelectedProducts: FC<{ selectedItems?: any[], setSelectedItems: (a: any[]) => void }> = ({
                                                                                                   selectedItems,
                                                                                                   setSelectedItems
                                                                                               }) => {
    const [modal, setModal] = useState<boolean>()
    return <ProductModal open={modal || false} handleClose={() => setModal(!modal)}>
        <div>
            this is modal
        </div>
    </ProductModal>
};

export default SelectedProducts;