
#pragma once
#include "stdafx.h"
#include "canvas.h"
#include "cachelib.h"
#include "animcontrol.h"
#include "manager.h"


class SBViewer: public wxFrame
{    
private:
    DECLARE_CLASS(SBViewer)
    DECLARE_EVENT_TABLE()
	//std::set<FileTreeItem> filelist;
	wxTreeCtrl *fileTree;
	FileType fileType;
	//wxWidget objects
	wxMenuBar *menuBar;
	wxMenu *fileMenu;
	wxMenu *viewMenu;
	wxMenu *helpMenu;	
	// wxAUI - new docking lib
	wxFrameManager interfaceManager;
	// Objects
	Canvas *canvas;

	AnimControl *animControl;


public:

	// Constructor + Deconstructor
	SBViewer(wxWindow* parent, wxWindowID id = -1, const wxString& caption = _T("SBViewer"),
		const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxDefaultSize, 
		long style = wxCAPTION|wxRESIZE_BORDER|wxSYSTEM_MENU);

	~SBViewer();
	
	// Initialising related functions
	void InitMenu();

	void Init();

	// Window GUI event related functions
	// void OnIdle();
	void OnClose(wxCloseEvent &event);
	
	void OnSize(wxSizeEvent &event);
    
	void OnExit(wxCommandEvent &event);
	
	void OnTreeSelect(wxTreeEvent &event);

    // menu commands
	void OnMenuCommand(wxCommandEvent &event);
	
	void OnMenuToggle(wxCommandEvent &event);
	
	void OnViewCommand(wxCommandEvent &event);

	// other
	void SaveFile(const wxChar *fn, unsigned int fileIndex);
};

