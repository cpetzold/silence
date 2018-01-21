using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	public static Transform[] GetChildren(this Transform transform) {
		Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < children.Length; i++) {
            children[i] = transform.GetChild(i);
        }
		return children;
	}

}