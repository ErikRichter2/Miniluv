using System;

public class EndsDef: BaseDef {
	public string Template;
	public string Text;
}

public class EndsDefinition : BaseDefinition<EndsDef>
{
	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			EndsDef item = new EndsDef ();
			item.Id = defId;
			item.Template = this.GetValue (defId, "End_template");
			item.Text = this.GetValue (defId, "End_text");
			this.Items.Add (item);
		}
	}
}

