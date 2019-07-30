using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT> third_count= new List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT>();

        public S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT(List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT> third_count)
        {
			this.third_count = third_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = third_count.Count;

			ListFormat listNode_THIRD_COUNT = ownerList.add(ListFormat.TYPE, -1, "THIRD_COUNT", "") as ListFormat;
			if(third_count != null)
			{
				foreach (S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT_THIRD_COUNT item in third_count)
				{
					listNode_THIRD_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}