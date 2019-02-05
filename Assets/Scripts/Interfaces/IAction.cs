using System.Collections;
using System.Collections.Generic;

public interface IAction {
    bool enabled { get; set; }
    bool CanDo();
    void Act();
}
