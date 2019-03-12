using System;
namespace TargetAudienceClient
{
	public interface IStatefulContent
	{
		void DidAppear();
		void DidDisappear();
	}
}
