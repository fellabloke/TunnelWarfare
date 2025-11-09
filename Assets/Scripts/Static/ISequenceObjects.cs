using UnityEngine;
using System.Collections;

public interface ISequenceObjects
{
    IEnumerator Execute(SkirmishOverseer manager, SkirmishContext context);
}