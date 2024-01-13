using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DevCommand : MonoBehaviour
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commanId{get {return _commandId;} }
    public string commanDescription{get {return _commandDescription;} }
    public string commanFormat{get {return _commandFormat;} }

    public DevCommand(string id,string description,string format){
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DevCommandB : DevCommand
{
    private Action command;

    public DevCommandB(string id,string description,string format,Action command) : base(id,description,format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}
