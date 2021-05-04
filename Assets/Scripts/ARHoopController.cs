using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class ARHoopController : MonoBehaviour
{
	[field: SerializeField]
	public static ARHoopController Instance { get; private set; }

	[field: SerializeField]
	public Transform ModelTransform { get; private set; }

	[field: SerializeField]
	public Material FrameMaterial { get; private set; }

	private ARFace ARFaceComponent { get; set; }

	private const string MATERIAL_COLOR_SETTING_NAME = "_Color";
	private const int AR_HOOP_PLACEMENT_VERTICE_INDEX = 16;

	public void ChangeFrameColor (Color color)
	{
		if (FrameMaterial != null)
		{
			FrameMaterial.SetColor(MATERIAL_COLOR_SETTING_NAME, color);
		}
	}

	protected virtual void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		ARFaceComponent = GetComponent<ARFace>();
	}

	protected virtual void OnDestroy ()
	{
		Instance = null;
	}

	protected virtual void OnEnable ()
	{
		ARFaceComponent.updated += TryToUpdateModelStatus;
		ARSession.stateChanged += TryToUpdateModelStatus;
		TryToUpdateModelStatus();
	}

	protected virtual void OnDisable ()
	{
		ARFaceComponent.updated -= TryToUpdateModelStatus;
		ARSession.stateChanged -= TryToUpdateModelStatus;
	}

	private void TryToUpdateModelStatus (ARFaceUpdatedEventArgs eventArgs)
	{
		TryToUpdateModelStatus();
	}

	private void TryToUpdateModelStatus (ARSessionStateChangedEventArgs eventArgs)
	{
		TryToUpdateModelStatus();
	}

	private void TryToUpdateModelStatus ()
	{
		bool isFaceVisible = GetFaceVisibility();
		ModelTransform.gameObject.SetActive(isFaceVisible);

		if (isFaceVisible == true)
		{
			ModelTransform.localPosition = ARFaceComponent.vertices[AR_HOOP_PLACEMENT_VERTICE_INDEX];
		}
	}

	private bool GetFaceVisibility()
    {
		return enabled == true && ARFaceComponent.trackingState != TrackingState.None && ARSession.state > ARSessionState.Ready;
	}
}