using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInitializer
{
	public class ModelBaseTree
	{
		public ModelBaseTree(ModelBase singleModel)
		{
			this.Model = singleModel;
			this.Children = new List<List<ModelBaseTree>>();
		}

		public ModelBaseTree(ModelBase singleModel, List<ModelBase> child)
		{
			this.Model = singleModel;
			var firstChild = new List<ModelBaseTree>();
			child.ForEach(x => firstChild.Add(new ModelBaseTree(x)));
			this.Children = new List<List<ModelBaseTree>>() {firstChild};
		}
		public ModelBase Model { get; set; }
		public List<List<ModelBaseTree>> Children { get; set; }
	}
}
