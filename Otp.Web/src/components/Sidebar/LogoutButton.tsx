import useModal from '@/hooks/useModal';
import msalInstance from '@/services/auth/msalInstance';
import { MsalService } from '@/services/auth/msalService';
import ConfirmationModal from '../ConfirmationModal/ConfirmationModal';
import LogoutIcon from '../misc/LogoutIcon';

const LogoutButton = () => {
	const { visible, toggle } = useModal();
	const handleOnLogout = async () => {
		const activeAccount = MsalService.getActiveAccount();

		msalInstance.logoutRedirect({
			account: activeAccount,
		});
		toggle();
	};

	return (
		<>
			<button className="btn btn-ghost gap-2 justify-start font-normal" onClick={toggle}>
				<LogoutIcon className="h-[24px] fill-current" />
				Log out
			</button>

			{visible && (
				<ConfirmationModal
					onConfirm={handleOnLogout}
					title="Are you sure you want to logout?"
					onClose={toggle}
					showModal={visible}
				/>
			)}
		</>
	);
};

export default LogoutButton;
