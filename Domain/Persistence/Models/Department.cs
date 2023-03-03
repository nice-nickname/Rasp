using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain;

public class Department : IncEntityBase
{
	public new virtual int Id { get; set; }

	public virtual string Name { get; set; }

	public virtual string Code { get; set; }

	public class Map : ClassMap<Department>
	{
		public Map()
		{
			Id(s => s.Id).GeneratedBy.Identity();
			Map(s => s.Name);
			Map(s => s.Code);
		}
	}
}