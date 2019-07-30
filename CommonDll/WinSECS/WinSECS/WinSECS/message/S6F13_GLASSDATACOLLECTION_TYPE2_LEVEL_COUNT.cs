using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_FIRST_COUNT> first_count= new List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_FIRST_COUNT>();
		private List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT> second_count= new List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT>();

        public S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT(List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_FIRST_COUNT> first_count, List<S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT> second_count)
        {
			this.first_count = first_count;
			this.second_count = second_count;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			ListFormat listNode_FIRST_COUNT = ownerList.add(ListFormat.TYPE, -1, "FIRST_COUNT", "") as ListFormat;
			if(first_count != null)
			{
				foreach (S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_FIRST_COUNT item in first_count)
				{
					listNode_FIRST_COUNT.add(item.getMessage(isNoPadding));
				}
			}
			ListFormat listNode_SECOND_COUNT = ownerList.add(ListFormat.TYPE, -1, "SECOND_COUNT", "") as ListFormat;
			if(second_count != null)
			{
				foreach (S6F13_GLASSDATACOLLECTION_TYPE2_LEVEL_COUNT_SECOND_COUNT item in second_count)
				{
					listNode_SECOND_COUNT.add(item.getMessage(isNoPadding));
				}
			}

            return ownerList;
        }

    }
}