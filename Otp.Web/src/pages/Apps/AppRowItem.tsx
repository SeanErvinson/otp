import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
import utc from 'dayjs/plugin/utc';
import { nanoid } from 'nanoid';
import { Link } from 'react-router-dom';

import { GetAppsApp } from '@/api/otpApi';
import { truncate } from '@/utils/stringUtils';

interface Props {
	app: GetAppsApp;
}

const maxTagCount = 3;
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
					{app.tags.slice(0, maxTagCount).map(tag => (
						<kbd key={nanoid()} className="kbd kbd-md">
							{tag}
						</kbd>
					))}
					{app.tags.length > maxTagCount && (
						<div data-tip={app.tags.slice(maxTagCount)} className="tooltip">
							<kbd className="kbd kbd-md">+{app.tags.length - maxTagCount}</kbd>
						</div>
					)}
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
