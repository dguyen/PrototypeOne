using System.Collections;
using System.Collections.Generic;

public interface IAction {
    bool CanDo();
    void Act();
}
