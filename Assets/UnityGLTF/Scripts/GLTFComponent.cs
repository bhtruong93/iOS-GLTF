using System.Collections;
using System.IO;
using UnityEngine;

namespace UnityGLTF
{

	/// <summary>
	/// Component to load a GLTF scene with
	/// </summary>
	class GLTFComponent : MonoBehaviour
	{
		public string Url;
		public bool Multithreaded = true;
		public bool UseStream = false;

		public int MaximumLod = 300;
		public GLTFSceneImporter.ColliderType Colliders = GLTFSceneImporter.ColliderType.None;

		IEnumerator Start()
		{
			GLTFSceneImporter loader = null;
			FileStream gltfStream = null;
			if (UseStream)
			{
				var fullPath = Path.Combine(Application.streamingAssetsPath, Url);
				gltfStream = File.OpenRead(fullPath);
				loader = new GLTFSceneImporter(
					fullPath,
					gltfStream,
					gameObject.transform,
                    Colliders
					);
			}
			else
			{
				loader = new GLTFSceneImporter(
					Url,
					gameObject.transform,
                    Colliders
					);
			}

            loader.MaximumLod = MaximumLod;
			yield return loader.Load(-1, Multithreaded);
			if (gltfStream != null)
			{
#if WINDOWS_UWP
				gltfStream.Dispose();
#else
				gltfStream.Close();
#endif
			}
		}
	}
}
