using System;

public class DaysDef: BaseDef {
	public int End;
	public int[] Next;
	public int[] ReqTasksOK;
	public int[] ReqTasksOK_OR;
	public int[] ReqTasksNOK;
	public int[] ReqTasksNOK_OR;
	public string NewsTemplate;
	public string NewsMainHeader;
	public string NewsMainText;
	public string NewsMainPicture;
	public string NewsSecondHeader;
	public string NewsThirdText;
	public string Telegram;
}

public class DaysDefinition : BaseDefinition<DaysDef>
{
	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			DaysDef item = new DaysDef ();
			item.Id = defId;
			item.End = this.GetValueInt (defId, "End");
			item.Next = this.GetValueArrayInt (defId, "Next");
			item.ReqTasksOK = this.GetValueArrayInt (defId, "Task_Id_OK");
			item.ReqTasksOK_OR = this.GetValueArrayInt (defId, "Task_Id_OK_OR");
			item.ReqTasksNOK = this.GetValueArrayInt (defId, "Task_Id_NOK");
			item.ReqTasksNOK_OR = this.GetValueArrayInt (defId, "Task_Id_NOK_OR");
			item.NewsTemplate = this.GetValue (defId, "News_Template");
			item.NewsMainHeader = this.GetValue (defId, "News_Main_Header");
			item.NewsMainText = this.GetValue (defId, "News_Main_Text");
			item.NewsMainPicture = this.GetValue (defId, "News_Main_Picture");
			item.NewsSecondHeader = this.GetValue (defId, "News_Second_Header");
			item.NewsThirdText = this.GetValue (defId, "News_Third_Text");
			item.Telegram = this.GetValue (defId, "Telegram");
			this.Items.Add (item);
		}
	}
}

