import { useQuery } from 'react-query';

const useApps = () =>
	useQuery(['getApp', appId], () => OtpApi.getApp(appId!), {
		enabled: !!appId,
		onError: (error: AxiosError<ProblemDetails>) => {
			if (
				error.response &&
				(error.response.status === 404 || error.response.status === 410)
			) {
				navigate('/404');
				return;
			}
		},
		onSuccess: data => {
			setSelectedApp(data);
		},
	});

export default useApps;
