#pragma once

#include "stdafx.h"
#include "sbviewer.h"
#include "resource.h"


// defines
const wxChar APP_TITLE[] = _T("Shadowbane Viewer v0.06");

class SBViewerApp : public wxApp
{
private:
	SBViewer *frame;
	FILE *logFile;

public:
    virtual bool OnInit();
	virtual int OnExit();
	virtual void OnUnhandledException();
	virtual void OnFatalException();
};