import DangerButton from '@/components/Buttons/DangerButton';
import { appsRoute } from '@/consts/endpoints';
import { useNavigate } from 'react-router-dom';
import useDeleteApp from '../../mutations/useDeleteApp';

const DangerSection = ({ appId }: { appId: string }) => {
	const mutation = useDeleteApp();
	const navigate = useNavigate();

	const handleOnConfirm = () => {
		mutation.mutate(
			{
				appId: appId,
			},
			{
				onSuccess: () => {
					navigate(appsRoute);
				},
			},
		);
	};

	return (
		<div className="max-w-lg mb-2">
			<h3 className="text-md font-semibold mb-2">Delete app</h3>
			<p className="text-sm mb-2">Once you delete an app, there is no going back.</p>
			<DangerButton
				label="Delete"
				onConfirm={handleOnConfirm}
				title="Are you sure you want to delete this app?"
				body="Any applications or scripts using this app will no longer be able to access the API. You cannot undo this action."
			/>
		</div>
	);
};

export default DangerSection;
