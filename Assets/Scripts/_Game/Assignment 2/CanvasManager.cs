using System.Collections;
using System.Collections.Generic;

using Jampacked.ProjectInca;
using Jampacked.ProjectInca.Events;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using UnityEngine.SceneManagement;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class ObjectiveCompleteEvent : Event<ObjectiveCompleteEvent>
	{
		public int objectiveId;
	}

	public class SceneResetEvent : Event<SceneResetEvent>
	{
	}

	public class CanvasManager : MonoBehaviour
	{
		public TMP_Text objective1;

		public TMP_Text time;
		public TMP_Text bestTime;

		public TMP_Text accuracy;
		public TMP_Text bestAccuracy;

		public Sprite jumpIcon;
		public Sprite wallrunIcon;
		public Sprite swapGunIcon;
		public Sprite shootEnemyIcon;
		public Sprite shootWeakspotIcon;
		public Sprite incompleteCheckbox;
		public Sprite completeCheckbox;
		public Sprite nextRoomIcon;

		public Image currentIcon;
		public Image currentCheckbox;

		public GameObject secondRunUI;
	}
}
