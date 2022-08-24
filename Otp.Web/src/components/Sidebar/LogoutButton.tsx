import useModal from '@/hooks/useModal';
import { MsalService } from '@/services/auth/msalService';

import ConfirmationModal from '../Modal/ConfirmationModal';
import LogoutIcon from '../misc/LogoutIcon';

const LogoutButton = () => {
	const { visible, toggle } = useModal();
	const handleOnLogout = async () => {
		MsalService.logout();
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
