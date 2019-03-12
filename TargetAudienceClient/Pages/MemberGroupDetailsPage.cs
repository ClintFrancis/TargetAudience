using System;
using TargetAudience.Common.Models;
using Xamarin.Forms;
using TargetAudience.Common.Utils;

namespace TargetAudienceClient.Pages
{
	public class MemberGroupDetailsPage : ContentPage, IStatefulContent
	{
		public MemberGroupDetailsPage(MemberGroup data)
		{
			Title = data.Gender.ToString() + " Audience";

			var total = new EntryCell { Label = "Total", Text = data.Total.ToString(), IsEnabled = false };
			var averageAge = new EntryCell { Label = "Average Age", Text = data.AverageAge.ToString(), IsEnabled = false };
			var averageSmile = new EntryCell { Label = "Average Smile", Text = data.AverageSmile.ToString(), IsEnabled = false };
			var averageFacialHair = new EntryCell { Label = "Average Facial Hair", Text = data.AverageFacialHair.ToString(), IsEnabled = false };
			var averageGlasses = new EntryCell { Label = "Average Glasses", Text = data.AverageGlasses.ToString(), IsEnabled = false };
			var averageMakeup = new EntryCell { Label = "Average Makeup", Text = data.AverageMakeup.ToString(), IsEnabled = false };
			var averageEmotion = new EntryCell { Label = "Average Emotion", Text = data.AverageEmotion, IsEnabled = false };

			var membersSection = new TableSection("Individuals");
			foreach (var member in data.Individuals)
			{
				string displayText = member.Emotion.PrimaryEmotion().ToEmoji();
				var memberCell = new EntryCell { Label = member.Age.ToString(), Text = displayText, IsEnabled = false };
				membersSection.Add(memberCell);
			}

			Content = new TableView
			{
				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection("Average Audience Data"){
					averageAge,
					averageSmile,
					averageFacialHair,
					averageGlasses,
					averageMakeup,
					averageEmotion
					},
					membersSection
				},
				Intent = TableIntent.Settings
			};


		}

		public void DidAppear()
		{
			throw new NotImplementedException();
		}

		public void DidDisappear()
		{
			throw new NotImplementedException();
		}
	}
}
