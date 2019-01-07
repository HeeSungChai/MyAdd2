public enum eTABLE_LIST
{
	CHAR_INFO,
	CHAR_LEVEL_ADD,
	CHAR_LEVEL_DIV,
	CHAR_LEVEL_MUL,
	CHAR_LEVEL_SUB,
	INFINITY_RANDOM,
	ITEM_ID,
	ITEM_TABLE,
	REWARD_TABLE,
	SALE_TYPE,
	STAGE_LEVEL_1,
	TITLE_MARK,
	END
}

public enum eKEY_TABLEDB
{
	f_ITEM_PRICE_US,
	f_SPEED_VALUE,
	i_AMOUNT,
	i_EXAM_FTRANDOM_MAX,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_NUM,
	i_ID,
	i_ITEM_ID,
	i_ITEM_LIMIT,
	i_ITEM_PRICE_TYPE,
	i_ITEM_SET_VALUE,
	i_LIFT_VALUE,
	i_LINE_ID,
	i_LV,
	i_NEED_ITEM,
	i_PERCENTAGE,
	i_REWARD_1,
	i_REWARD_1_VALUE,
	i_REWARD_2,
	i_REWARD_2_VALUE,
	i_REWARD_3,
	i_REWARD_3_VALUE,
	i_REWARD_AMOUNT,
	i_REWARD_ID,
	i_SKILL_VALUE,
	i_SPEED_VALUE,
	i_STAGE,
	i_TYPE,
	i_TYPE_VALUE,
	i_UNLOCK_ID,
	i_UNLOCK_STAGE_INFO,
	s_APPEARTIME,
	s_CHAR_NAME_KR,
	s_CHAR_NAME_US,
	s_CHAR_STORY_KR,
	s_CHAR_STORY_US,
	s_CONDITION,
	s_GRADE_NAME_KR,
	s_GRADE_NAME_US,
	s_ITEM_INFO_KR,
	s_ITEM_INFO_US,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	s_RESOURCE,
	s_SKILL_KR,
	s_SKILL_US,
	s_TITLE_NAME_KR,
	s_TITLE_NAME_US,
	END
}

public enum CHAR_INFO
{
	i_ID,
	s_CHAR_NAME_KR,
	s_CHAR_NAME_US,
	s_CHAR_STORY_KR,
	s_CHAR_STORY_US,
	s_SKILL_KR,
	s_SKILL_US,
	i_UNLOCK_STAGE_INFO,
	END
}

public enum CHAR_LEVEL_ADD
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFT_VALUE,
	END
}

public enum CHAR_LEVEL_DIV
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFT_VALUE,
	END
}

public enum CHAR_LEVEL_MUL
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFT_VALUE,
	END
}

public enum CHAR_LEVEL_SUB
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFT_VALUE,
	END
}

public enum INFINITY_RANDOM
{
	i_SPEED_VALUE,
	i_PERCENTAGE,
	END
}

public enum ITEM_ID
{
	i_ID,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	END
}

public enum ITEM_TABLE
{
	i_ID,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	s_ITEM_INFO_KR,
	s_ITEM_INFO_US,
	i_ITEM_ID,
	i_ITEM_SET_VALUE,
	i_ITEM_LIMIT,
	i_ITEM_PRICE_TYPE,
	f_ITEM_PRICE_US,
	END
}

public enum REWARD_TABLE
{
	i_STAGE,
	i_REWARD_1,
	i_REWARD_1_VALUE,
	i_REWARD_2,
	i_REWARD_2_VALUE,
	i_REWARD_3,
	i_REWARD_3_VALUE,
	i_UNLOCK_ID,
	END
}

public enum SALE_TYPE
{
	i_TYPE,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	END
}

public enum STAGE_LEVEL_1
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum TITLE_MARK
{
	i_ID,
	s_TITLE_NAME_KR,
	s_TITLE_NAME_US,
	s_GRADE_NAME_KR,
	s_GRADE_NAME_US,
	s_RESOURCE,
	s_CONDITION,
	i_REWARD_ID,
	i_REWARD_AMOUNT,
	END
}
