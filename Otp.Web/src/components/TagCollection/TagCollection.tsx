import { nanoid } from 'nanoid';

const maxTagCount = 3;

interface Props {
	tags: string[];
	tagCount?: number;
}

const TagCollection = ({ tags, tagCount = maxTagCount }: Props) => {
	return (
		<>
			{tags.slice(0, tagCount).map(tag => (
				<span key={nanoid()} className="badge badge-md">
					{tag}
				</span>
			))}
			{tags.length > tagCount && (
				<div data-tip={tags.slice(tagCount)} className="tooltip">
					<span className="badge badge-md">+{tags.length - tagCount}</span>
				</div>
			)}
		</>
	);
};

export default TagCollection;
