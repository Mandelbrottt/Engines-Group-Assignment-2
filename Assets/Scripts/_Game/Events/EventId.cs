using UnityEngine;

namespace Jampacked.ProjectInca.Events
{
	public sealed class EventId
	{
		private static int s_uniqueEventId = 0;

		private readonly int m_uniqueEventId;

		public EventId()
		{
			m_uniqueEventId = s_uniqueEventId++;
		}

		public EventId(EventId a_other)
		{
			m_uniqueEventId = a_other.m_uniqueEventId;
		}
		
	#region Equality members
		public bool Equals(EventId a_other)
		{
			return m_uniqueEventId == a_other.m_uniqueEventId;
		}

		public override bool Equals(object a_obj)
		{
			if (ReferenceEquals(null, a_obj))
			{
				return false;
			}
			
			if (ReferenceEquals(this, a_obj))
			{
				return true;
			}
			
			if (a_obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((EventId) a_obj);
		}

		public override int GetHashCode()
		{
			return m_uniqueEventId;
		}

		public static bool operator==(EventId a_lhs, EventId a_rhs)
		{
			bool lhsNull = ReferenceEquals(a_lhs, null);
			bool rhsNull = ReferenceEquals(a_rhs, null);

			if (lhsNull && rhsNull)
			{
				return true;
			}

			if (lhsNull || rhsNull)
			{
				return false;
			}

			return a_lhs.Equals(a_rhs);
		}

		public static bool operator!=(EventId a_lhs, EventId a_rhs)
		{
			return !(a_lhs == a_rhs);
		}
	#endregion
	}
}