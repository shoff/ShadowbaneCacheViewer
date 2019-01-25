// SBCacheViewer.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "SBCacheViewer.h"

// Library files required for this app
#ifdef _WIN32
	#pragma message("     Adding library: uxtheme.lib" ) 
	#pragma comment( lib, "uxtheme.lib" ) // WinXP Theme Engine
	#pragma message("     Adding library: comctl32.lib" ) 
	#pragma comment( lib, "comctl32.lib" ) // Common Controls 32bit
	#pragma message("     Adding library: rpcrt4.lib" ) 
	#pragma comment( lib, "rpcrt4.lib" )

	//#pragma message("     Adding library: fmod32.lib" ) 
	//#pragma comment( lib, "fmodvc.lib" ) // FMOD (sound) lib

	#ifdef _DEBUG
		#ifdef __WXMSW__
			#ifdef _UNICODE
				#pragma message("     Adding Debug Unicode wxWidget libraries" ) 
				#pragma comment( lib, "wxmsw26ud_core.lib" ) // wxCore Unicode Debug lib
				#pragma comment( lib, "wxmsw26ud_adv.lib" )
				#pragma comment( lib, "wxmsw26ud_gl.lib" )
				#pragma comment( lib, "wxmsw26ud_qa.lib" )
			#else
				#pragma message("     Adding Debug Ascii wxWidget libraries" ) 
				#pragma comment( lib, "wxmsw26d_core.lib" )	// wxCore Debug Lib
				#pragma comment( lib, "wxmsw26d_adv.lib" )
				#pragma comment( lib, "wxmsw26d_gl.lib" )	// wxGLCanvas Debug lib
				#pragma comment( lib, "wxmsw26d_qa.lib" )
			#endif
		#elif __WXMAC__
			#pragma comment( lib, "wxmac26d_core.lib" )
			#pragma comment( lib, "wxmac26d_adv.lib" )
			#pragma comment( lib, "wxmac26d_gl.lib" )
			#pragma comment( lib, "wxmac26d_qa.lib" )
		#endif

		#ifdef _UNICODE
			#pragma comment( lib, "wxregexud.lib" )
			#pragma comment( lib, "wxbase26ud.lib" )
			#pragma message("     Adding library: wxauiud.lib" ) 
			#pragma comment( lib, "wxauiud.lib" )		// wxAUI
		#else 
			#pragma comment( lib, "wxregexd.lib" )
			//#pragma comment( lib, "wxbase26d.lib" )
			#pragma message("     Adding library: wxauid.lib" ) 
			#pragma comment( lib, "wxauiud.lib" )		// wxAUI
		#endif

		#pragma comment( lib, "wxzlibd.lib" )
		#pragma message("     Adding library: cximagecrtd.lib" ) 
		#pragma comment( lib, "cximage.lib" )	// cxImage

	// release
	#else
		
		#define NDEBUG			// Disables Asserts in release
		#define VC_EXTRALEAN	// Exclude rarely-used stuff from Windows headers
		#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

		#ifdef _WIN32
			#pragma message("	Adding Release Ascii wxWidget libraries" ) 
			#pragma comment( lib, "wxmsw26_core.lib" )
			#pragma comment( lib, "wxmsw26_adv.lib" )
			#pragma comment( lib, "wxmsw26_gl.lib" )
			#pragma comment( lib, "wxmsw26_qa.lib" )
		#endif
		
		#pragma comment( lib, "wxzlib.lib" )
		#pragma comment( lib, "wxregex.lib" )
		#pragma comment( lib, "wxbase26.lib" )

		#pragma message("     Adding library: wxaui.lib" ) 
		#pragma comment( lib, "wxaui.lib" )
		#pragma message("     Adding library: cximagecrt.lib" ) 
		#pragma comment( lib, "cximagecrt.lib" )
	#endif
#endif



// tell wxwidgets which class is our app
IMPLEMENT_APP(SBViewerApp)

bool SBViewerApp::OnInit()
{
	frame = NULL;
	logFile = NULL;

	// Error & Logging settings
#ifndef _DEBUG
	#if wxUSE_ON_FATAL_EXCEPTION
		wxHandleFatalExceptions(true);
	#endif
#endif

	// Create our log file
	logFile = fopen("log.txt", "w+");
	if (logFile) 
	{
		wxLog *logger = new wxLogStderr(logFile);
		delete wxLog::SetActiveTarget(logger);
		wxLog::SetVerbose(true);
	}

	// Application Info
	SetVendorName(_T("SBViewer"));
	SetAppName(_T("SBViewer"));

	// Just a little header to start off the log file.
	wxLogMessage(_T("Starting... \n%s\n---\n"), APP_TITLE);
	
	// Now create our main frame.
    frame = new SBViewer(NULL, wxID_ANY, APP_TITLE, wxDefaultPosition, wxSize(1024, 768), wxDEFAULT_FRAME_STYLE );
    
	if (!frame) 
	{
		//this->Close();
		return false;
	}
	
	// Set frame as the primary/parent window
	SetTopWindow(frame);

	// Set the icon, different source location for the icon under linux
	wxIcon icon("SBCacheViewer.ico");
	frame->SetIcon(icon);
	// --
	wxLogMessage(_T("SB Viewer successfully loaded!\n----\n"));
	return true;
}

void SBViewerApp::OnFatalException()
{
	
	//wxApp::SetExitOnFrameDelete(false);

	wxDebugReport report;
    wxDebugReportPreviewStd preview;

	report.AddAll(wxDebugReport::Context_Exception);

    if (preview.Show(report))
        report.Process();

	if (frame) {
		frame->Destroy();
		frame = NULL;
	}
	
}

int SBViewerApp::OnExit()
{	
	//if (frame != NULL)
	//	frame->Destroy();

//#ifdef _DEBUG
	//delete wxLog::SetActiveTarget(NULL);
	if (logFile) {
		fclose(logFile);
		//wxDELETE(LogFile);
		logFile = NULL;
	}
//#endif

	CleanUp();

	return 0;
}


void SBViewerApp::OnUnhandledException() 
{ 
    //wxMessageBox(_T("An unhandled exception was caught, the program will now terminate."), _T("Unhandled Exception"), wxOK | wxICON_ERROR); 
	wxLogFatalError(_T("An unhandled exception error has occured."));
} 


