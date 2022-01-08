import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
import utc from 'dayjs/plugin/utc';
import { Link } from 'react-router-dom';

import { GetAppsApp } from '@/api/otpApi';
import { truncate } from '@/utils/stringUtils';
import { TagCollection } from '@/components/TagCollection';

interface Props {
	app: GetAppsApp;
}

const maxDescriptionLength = 32;

dayjs.extend(utc);
dayjs.extend(relativeTime);

const AppRowItem = ({ app }: Props) => {
	return (
		<tr className="">
			<td className="flex flex-col h-full">
				<span className="font-semibold">{app.name}</span>
				<span className="text-md opacity-75">
					{app.description ? truncate(app.description, maxDescriptionLength) : '---'}
				</span>
			</td>
			<td>{dayjs().to(dayjs.utc(app.createdAt).local())}</td>
			<td>
				<div className="flex flex-row gap-2">
					{app.tags && <TagCollection tags={app.tags} />}
				</div>
			</td>
			<th>
				<Link className="btn btn-ghost btn-xs" to={`/apps/${app.id}`}>
					details
				</Link>
			</th>
		</tr>
	);
};

export default AppRowItem;
