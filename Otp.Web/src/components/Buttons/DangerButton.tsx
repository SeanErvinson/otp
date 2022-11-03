import useModal from '@/hooks/useModal';
import ConfirmationModal, { ConfirmationProps } from '../Modal/ConfirmationModal';

interface ButtonProps {
	label: string;
	onClick?: () => void;
}

interface Props extends ButtonProps {
	title: string;
	body: string;
	onConfirm: () => void;
}

const DangerButton = (props: Props) => {
	const { toggle, visible } = useModal();

	return (
		<>
			<button className="btn
             btn-error" onClick={toggle}>
				{props.label}
			</button>
			{visible && (
				<ConfirmationModal
					onClose={toggle}
					showModal={visible}
					title={props.title}
					body={props.body}
					onConfirm={props.onConfirm}
					confirmActionLabel="I understand"
					confirmLoadingActionLabel="Deleting"
				/>
			)}
		</>
	);
};

export default DangerButton;
