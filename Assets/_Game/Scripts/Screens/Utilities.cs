using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
{
    #region Member Variables

	private static StringBuilder stringBuilder = new StringBuilder();

    #endregion

    #region Properties

    public static double SystemTimeInMilliseconds { get { return (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMilliseconds; } }

	public static float WorldWidth { get { return 2f * Camera.main.orthographicSize * Camera.main.aspect; } }
	public static float WorldHeight { get { return 2f * Camera.main.orthographicSize; } }
	public static float XScale { get { return (float)Screen.width / 1080f; } }
	public static float YScale { get { return (float)Screen.height / 1920f; } }

	#endregion

	#region Public Methods

	/// <summary>
	/// Returns to mouse position
	/// </summary>
	public static Vector2 MousePosition()
	{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		return (Vector2)Input.mousePosition;
#else
			if (Input.touchCount > 0)
			{
				return Input.touches[0].position;
			}

			return Vector2.zero;
#endif
	}

	/// <summary>
	/// Returns true if a mouse down event happened, false otherwise
	/// </summary>
	public static bool MouseDown()
	{
		return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began);
	}

	/// <summary>
	/// Returns true if a mouse up event happened, false otherwise
	/// </summary>
	public static bool MouseUp()
	{
		return (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended));
	}

	/// <summary>
	/// Returns true if no mouse events are happening, false otherwise
	/// </summary>
	public static bool MouseNone()
	{
		return (!Input.GetMouseButton(0) && Input.touchCount == 0);
	}

	public static char CharToLower(char c)
	{
		return (c >= 'A' && c <= 'Z') ? (char)(c + ('a' - 'A')) : c;
	}

	public static Canvas GetCanvas(Transform transform)
	{
		if (transform == null)
		{
			return null;
		}

		Canvas canvas = transform.GetComponent<Canvas>();

		if (canvas != null)
		{
			return canvas;
		}

		return GetCanvas(transform.parent);
	}

	public static void SetLayer(GameObject gameObject, int layer, bool applyToChildren = false)
	{
		gameObject.layer = layer;

		if (applyToChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				SetLayer(gameObject.transform.GetChild(i).gameObject, layer, true);
			}
		}
	}

	public static Vector2 Rotate(Vector2 v, float degrees)
	{
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;

		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);

		return v;
	}

	public static void SwapValue<T>(ref T value1, ref T value2)
	{
		T temp = value1;
		value1 = value2;
		value2 = temp;
	}

	public static float EaseOut(float t)
	{
		return 1.0f - (1.0f - t) * (1.0f - t) * (1.0f - t);
	}

	public static float EaseIn(float t)
	{
		return t * t * t;
	}

	public static void DeactivateObjectInList<T>(this List<T> listObject) where T : Component
	{
		foreach (T obj in listObject) obj.gameObject.SetActive(false);
	}

	public static T CloneObjectInList<T>(this List<T> listObject, T prefab, Transform parent, bool ignoreDeactive = false) where T : Component
	{
		if (!ignoreDeactive)
        {
			foreach (T obj in listObject)
			{
				if (obj.gameObject.activeSelf) continue;
				obj.transform.SetParent(parent);
				obj.gameObject.SetActive(true);
				return obj;
			}
		}
		
		T cloneObject = Object.Instantiate(prefab);
		cloneObject.transform.SetParent(parent);
		cloneObject.name = prefab.name;
		listObject.Add(cloneObject);
		return cloneObject;
	}
	public static void SaveTextureToFile (Texture2D texture2D, string fileName, string dataPath = "")
	{
		var bytes = texture2D.EncodeToPNG();
		if (dataPath.Equals(string.Empty))
        {
			dataPath = Application.dataPath;
        }
		var file = File.Open(dataPath + "/" + fileName + ".png", FileMode.Create);
		var binary = new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();
	}

	public static string ConvertToDateTimeFormat(int totalSeconds)
	{
		stringBuilder.Clear();
		int hours = totalSeconds / 3600;
		int minutes = (totalSeconds % 3600) / 60;
		int seconds = totalSeconds % 60;

		if (hours > 0) stringBuilder.AppendFormat("{0}h", hours);
		if (minutes > 0) stringBuilder.AppendFormat(" {0}m", minutes);
		if (seconds > 0) stringBuilder.AppendFormat(" {0}s", seconds);
		return stringBuilder.ToString();
	}

	public static string RandomName()
    {
		string[] nameList = new string[] {
			"Lennon Marsh", "Bo Rodgers", "Selah Hahn", "Kabir Whitehead", "Sylvie Stokes", 
			"Santana Simpson", "Anastasia Wilkerson", "Carmelo Dorsey", "Addyson Sanford",
			"Truett Garza", "River Sellers", "Madden Giles", "Bailee McCann", "Heath Whitaker",
			"Ivanna Ventura", "Branson Tanner", "Harmoni Doyle", "Kashton Glass",
			"Clare Meza", "Lucian Ponce", "Aileen Larson", "Rafael Galvan", "Dallas Coleman",
			"Micah Wagner", "Maeve Hail", "Hector Padilla", "Maggie Coleman", "Micah Flores",
			"Emilia Houston", "Sylas Leach", "Martha Portillo", "Wallace Baldwin",
			"Esmeralda May", "Finley Escobar", "Erin O’Donnell", "Lian Boone", "Mariam Avalos",
			"Coen Collier", "Ivory Kaur", "Augustine Cabrera", "Daleyza Estrada", "Phoenix Ross",
			"Peyton McFarland", "Dane Aguilar", "Josie House", "Yehuda Preston", "Indie Bass",
			"Landen Chandler", "Viviana Brandt", "Damir Roberts", "Paisley O’brien",
			"Riley Wang", "Kailani Macdonald", "Hugh Baxter", "Lara Patel", "Parker Walter",
			"Penny Greer", "Koda Macdonald", "Rosalia Vaughn", "Remy Norman", "Malani Singleton",
			"Landyn Chan", "Hattie Harmon", "Roberto Malone", "Skyler Scott", "Leo Luna",
			"Journey Stevens", "Zachary Huff", "Karsyn Huffman", "Chris Sanford", "Emerald Holt",
			"Niko Leal", "Murphy Kane", "Brock Robles", "Felicity Rice", "Graham Combs",
			"Irene Buckley", "Aryan Stark", "Kamilah Powers", "Sean Skinner", "Mara Pham",
			"Russell Delacruz", "Celine Gould", "Blaine Lyons", "Kenzie Austin", "Omar Rosas",
			"Joelle Valencia", "Dax Bradley", "Vanessa Juarez", "Joaquin Hurst", "Adalee O’brien",
			"Riley Gould", "Violeta Peralta", "Dangelo Reilly", "Tori Lynn", "Zechariah Foley",
			"Zaylee Moran", "Tate Vazquez", "Journee Rogers", "Colton Walter", "Penny Yu"
		};
		int rand = Random.Range(0, nameList.Length);
		return nameList[rand];
    }
	#endregion

}

public static class RectTransformExtensions
{
	public static void SetLeft(this RectTransform rt, float left)
	{
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(this RectTransform rt, float right)
	{
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(this RectTransform rt, float top)
	{
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(this RectTransform rt, float bottom)
	{
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}
}
