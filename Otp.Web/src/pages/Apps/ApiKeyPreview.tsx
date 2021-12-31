import useClippy from 'use-clippy';

interface Props {
	apiKey: string;
	onClose: () => void;
}

const ApiKeyPreview = (props: Props) => {
	const [, setClipboard] = useClippy();

	return (
		<>
			<div>
				<h3 className="text-xl font-semibold">API Key</h3>
				<br />
				<p className="text-md">
					Please, make sure you have copied the API key. You won’t be able to see it
					again!
				</p>
			</div>
			<div className="relative">
				<input
					type="text"
					readOnly
					defaultValue={props.apiKey}
					className="w-full pr-16 input input-primary input-bordered"
				/>
				<button
					className="absolute top-0 right-0 rounded-l-none btn btn-primary"
					onClick={() => setClipboard(props.apiKey)}>
					Copy
				</button>
			</div>
			<button className="btn btn-accent" type="button" onClick={props.onClose}>
				Yes, I've copied it.
			</button>
		</>
	);
};

export default ApiKeyPreview;
