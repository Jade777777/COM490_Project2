using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.
public class MultipleTracker : MonoBehaviour
{
    private ARTrackedImageManager imageManager;
    private void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        imageManager.trackedImagesChanged += OnTrackedImage;
    }
    public void OnTrackedImage(ARTrackablesParentTransformChangedEventArgs args)
    {
        foreach(ARTrackedImage trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;
            GameObject imagePrefab = Resources.Load<GameObject>(imageName);

            if(imagePrefab != null) 
            {
                if (trackedImage.transform.childCount < 1)
                {
                    GameObject go = Instantiate(imagePrefab, trackedImage.transform.position, trackedImage.transform.rotation);

                    go.transform.SetParent(trackedImage.transform);
                }

            }

        }

        foreach(ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.transform.childCount>0)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;

            }
        }


    }

}
