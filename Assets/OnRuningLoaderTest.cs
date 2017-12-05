using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using UnityEngine;
using System;
using System.IO;

public class OnRuningLoaderTest : MonoBehaviour {

    private CubismModel3Json jsondata;
	private CubismModel cmodel;

	public string loadingModelJsonPath = "write your local path";

    // Use this for initialization
    void Start () {
        jsondata = CubismModel3Json.LoadAtPath(loadingModelJsonPath, normalFileLoad);
        cmodel = jsondata.ToModel();

		var renderController = cmodel.gameObject.GetComponent<CubismRenderController>();
		renderController.SortingMode = CubismSortingMode.BackToFrontOrder;

		var AutoBlinkController = cmodel.gameObject.AddComponent<CubismAutoEyeBlinkInput>();
		AutoBlinkController.enabled = true;

		var BlinkController = cmodel.gameObject.GetComponent<CubismEyeBlinkController>();
		BlinkController.BlendMode = CubismParameterBlendMode.Override;


		var animeController = cmodel.gameObject.AddComponent<Animation>();
		animeController.playAutomatically = false;

		var nowPlayAudio = cmodel.gameObject.AddComponent<AudioSource>();
		nowPlayAudio.playOnAwake = false;

		var breathController = cmodel.gameObject.AddComponent<CubismBreathController>();
		var breathParam = cmodel.Parameters.FindById("PARAM_BREATH");
		breathController.enabled = true;
		breathParam.gameObject.AddComponent<CubismBreathParameter>();		

	}

	// Update is called once per frame
	void Update () {
		
	}


    private static object normalFileLoad(Type assetType, string assetPath)
    {
        // Explicitly deal with byte arrays.
        if (assetType == typeof(byte[]))
        {
            return File.ReadAllBytes(assetPath);
        }
        else if (assetType == typeof(string))
        {
                return File.ReadAllText(assetPath);
        }
        else if (assetType == typeof(Texture2D))
        {
            Texture2D r = new Texture2D(0, 0);
            r.LoadImage(File.ReadAllBytes(assetPath));

            return r;
        }

        return File.ReadAllBytes(assetPath);
    }

}
