using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain;

public class Building : IncEntityBase
{
	public new virtual int Id { get; set; }

	public virtual string Code { get; set; }

	public class Map : ClassMap<Building>
	{
		public Map()
		{
			Id(s => s.Id).GeneratedBy.Identity();
			Map(s => s.Code);
		}
	}
}