
#include "stdafx.h"
#include "sbviewer.h"
#include "objects.h"
// FMod
//#include <fmod.h>

class FileTreeData:public wxTreeItemData
{
public:
	unsigned int id;
	bool zone;

	FileTreeData(unsigned int id, bool zone=false) : 
	id(id), zone(zone) 
	{
	}
};


IMPLEMENT_CLASS(SBViewer, wxFrame)

BEGIN_EVENT_TABLE(SBViewer, wxFrame)
EVT_CLOSE(SBViewer::OnClose)
EVT_SIZE(SBViewer::OnSize)

// File menu
EVT_MENU(ID_FILE_OPEN, SBViewer::OnMenuCommand)
EVT_MENU(ID_FILE_OPENALL, SBViewer::OnMenuCommand)
//--
EVT_MENU(ID_FILE_SAVE, SBViewer::OnMenuCommand)
// --
EVT_MENU(ID_FILE_EXIT, SBViewer::OnExit)

//Ground menu
EVT_MENU(ID_GROUND_DRAW, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_GRASS, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_DESERT, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_UNDEAD, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_VOLCANIC, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_SNOW, SBViewer::OnViewCommand)
EVT_MENU(ID_GROUND_CHECKER, SBViewer::OnViewCommand)

// View Menu
EVT_MENU(ID_VIEW_WIREFRAME, SBViewer::OnViewCommand)
EVT_MENU(ID_VIEW_SOLID, SBViewer::OnViewCommand)
// --
EVT_MENU(ID_VIEW_LIGHT, SBViewer::OnViewCommand)
EVT_MENU(ID_VIEW_BOUNDS, SBViewer::OnViewCommand)
EVT_MENU(ID_VIEW_WALKABLE, SBViewer::OnViewCommand)
EVT_MENU(ID_VIEW_SKELETON, SBViewer::OnViewCommand)

// file tree
EVT_TREE_SEL_CHANGED(ID_SBVIEWER_FILELIST, SBViewer::OnTreeSelect)
END_EVENT_TABLE()



SBViewer::SBViewer(wxWindow* parent, wxWindowID id, const wxString& caption, const wxPoint& pos, const wxSize& size, long style)
{
	canvas = 0;
	animControl = 0;
	menuBar = 0;
	fileMenu = 0;
	viewMenu = 0;
	helpMenu = 0;
	fileTree = 0;

	// create our main frame
	if (Create(parent, id, caption, pos, size, style,_T("SBViewerFrame"))) {
		// Display the window
		Centre();

		//InitObjects();  // create our canvas, anim control, character control, etc

		// Show our window
		Show(true);
	} else {
		wxLogMessage(_T("Critical Error: Unable to create the main window for the application to use!"));
		Close(true);
		return;
	}

	// Initialise the window
	InitMenu();
	Init();

}

void SBViewer::Init()
{
	wxLogMessage(_T("Initiating SB Viewer Frame."));

	// Create objects
	fileTree = new wxTreeCtrl(this, ID_SBVIEWER_FILELIST, wxDefaultPosition, wxSize(100,700), wxTR_SINGLE|wxTR_HIDE_ROOT|wxTR_HAS_BUTTONS|wxTR_LINES_AT_ROOT);
	fileTree->AddRoot(_T("Root"));

	canvas = new Canvas(this);

	animControl = new AnimControl(this, ID_FRAME_ANIM);



	// wxAUI stuff
	interfaceManager.SetFrame(this);

	// Filetree
	interfaceManager.AddPane(fileTree, wxPaneInfo().
		Name(_T("fileTree")).Caption(_T("File List")).
		BestSize(wxSize(170,700)).Left().Layer(1));

	// Animation control
	interfaceManager.AddPane(animControl, wxPaneInfo().
		Name(wxT("animControl")).Caption(_("Animation")).
		Bottom().Layer(0));

	// Canvas
	interfaceManager.AddPane(canvas, wxPaneInfo().
		Name(wxT("Canvas")).Caption(_T("Canvas")).
		Center().Layer(1).CloseButton(false).
		PaneBorder(false).CaptionVisible(false).Show(true));

	interfaceManager.Update();
}

void SBViewer::InitMenu()
{
	wxLogMessage(_T("Initiating File Menu."));

	// File menu
	fileMenu = new wxMenu;
	fileMenu->Append(ID_FILE_OPEN, _T("&Open\tCTRL+O"));
	fileMenu->Append(ID_FILE_OPENALL, _T("&Open All"));
	fileMenu->AppendSeparator();
	fileMenu->Append(ID_FILE_SAVE, _T("&Save\tCTRL+S"));
	fileMenu->AppendSeparator();
	fileMenu->Append(ID_FILE_EXIT, _T("E&xit\tCTRL+X"));

	// ground menu
	wxMenu *groundMenu = new wxMenu;
	groundMenu->AppendCheckItem(ID_GROUND_DRAW, _T("Draw Ground"));
	groundMenu->Check(ID_GROUND_DRAW, false);
	groundMenu->AppendSeparator();
	groundMenu->AppendRadioItem(ID_GROUND_GRASS, _T("Grass"));
	groundMenu->AppendRadioItem(ID_GROUND_DESERT, _T("Desert"));
	groundMenu->AppendRadioItem(ID_GROUND_VOLCANIC, _T("Volcanic"));
	groundMenu->AppendRadioItem(ID_GROUND_UNDEAD, _T("Undead"));
	groundMenu->AppendRadioItem(ID_GROUND_SNOW, _T("Snow"));
	groundMenu->AppendRadioItem(ID_GROUND_CHECKER, _T("Checker"));
	groundMenu->Check(ID_GROUND_CHECKER, true);


	// View menu
	viewMenu = new wxMenu;
	viewMenu->AppendRadioItem(ID_VIEW_WIREFRAME, _T("Wireframe"));
	viewMenu->AppendRadioItem(ID_VIEW_SOLID, _T("Solid"));
	viewMenu->Check(ID_VIEW_SOLID, true);
	viewMenu->AppendSeparator();
	viewMenu->AppendCheckItem(ID_VIEW_LIGHT, _T("Lighting"));
	viewMenu->Check(ID_VIEW_LIGHT, false);
	viewMenu->AppendCheckItem(ID_VIEW_BOUNDS, _T("Bounds"));
	viewMenu->Check(ID_VIEW_BOUNDS, false);
	viewMenu->AppendCheckItem(ID_VIEW_WALKABLE, _T("Walkable"));
	viewMenu->Check(ID_VIEW_WALKABLE, false);
	viewMenu->AppendCheckItem(ID_VIEW_SKELETON, _T("Skeleton"));
	viewMenu->Check(ID_VIEW_SKELETON, false);
	viewMenu->AppendSeparator();
	viewMenu->Append(ID_VIEW_GROUND, _T("Ground"), groundMenu);


	// Menu bar
	menuBar = new wxMenuBar();
	menuBar->Append(fileMenu, _T("&File"));
	menuBar->Append(viewMenu, _T("&View"));
	SetMenuBar(menuBar);
}

// This is called when the user goes to File->Exit
void SBViewer::OnExit(wxCommandEvent &event)
{
	if (event.GetId() == ID_FILE_EXIT)
		Close(false);
}

// This is called when the window is closing
void SBViewer::OnClose(wxCloseEvent &event)
{
	Destroy();
}

// Called when the window is resized, minimised, etc
void SBViewer::OnSize(wxSizeEvent &event)
{

}

void SBViewer::OnMenuCommand(wxCommandEvent &event)
{
	// Creates and pushes an hourglass cursor on the stack
	wxBusyCursor wait;

	// Open *.cache file
	if (event.GetId() == ID_FILE_OPEN) {
		wxFileDialog dialog(this, _T("Select *.Cache File"), 
			wxEmptyString, wxEmptyString, _T("Cache files (*.cache)|*.cache"), wxOPEN);

		if (dialog.ShowModal()==wxID_OK) 
		{

			// Error check
			if (canvas) {
				// Clear old
				wxDELETE(canvas->tex);
				wxDELETE(canvas->model);
				wxDELETE(canvas->skeleton);
				wxDELETE(canvas->obj);
			}

			wxString fn(dialog.GetPath());

			cArchive.open(fn.c_str());

			// Find out what type of cache file was opened.
			fn = fn.AfterLast('\\');
			fn.LowerCase();
			if (fn == _T("textures.cache"))
				fileType = CACHE_TEXTURE;
			else if (fn == _T("terrainalpha.cache"))
				fileType = CACHE_TEXTURE;
			else if (fn == _T("tile.cache"))
				fileType = CACHE_TEXTURE;
			else if (fn == _T("mesh.cache"))
				fileType = CACHE_MESH;
			else if (fn == _T("czone.cache"))
				fileType = CACHE_ZONE;
			else if (fn == _T("skeleton.cache")) {
				fileType = CACHE_SKELETON;

				wxString motion = dialog.GetPath();
				motion = motion.BeforeLast('\\');
				motion.Append("\\motion.cache");
				motionArchive.open(motion.fn_str());

			} else if (fn == _T("cobjects.cache"))
				fileType = CACHE_OBJECT;
			else if (fn == _T("sound.cache")) {



				//FMOD_RESULT init(
				//	int  maxchannels, 
				//	FMOD_INITFLAGS  flags, 
				//	void *  extradriverdata
				//	);


				//// Initiate FMod sound API
				//if (!FSOUND_Init(44100, 32, 0))
				//	wxLogMessage(_T("Error: Failed to initiate fmod!"));
				//else
				//	wxLogMessage(_T("FMod Audio API initialised."));

				//fileType = CACHE_SOUND;

			} else
				fileType = CACHE_OTHER;

			fileTree->Unselect();
			fileTree->DeleteAllItems();
			wxTreeItemId root = fileTree->AddRoot(_T("Root"));

			unsigned int fileCount = cArchive.getFileCount();

			if (fileType == CACHE_OBJECT)
			{
				wxString name; 
				unsigned char *tempBuffer = NULL;  
				unsigned char letter;  
				unsigned int size;
				//unsigned int flag;

				for (unsigned int t=0; t<fileCount; t++) 
				{ 
					int val = cArchive.getFileID(t);
					tempBuffer = cArchive.getFile(t); 
					//memcpy(&flag, tempBuffer + 4, 4);  
					memcpy(&size, tempBuffer + 8, 4);  

					for(unsigned int i = 0; i<size; i++)  
					{
						memcpy(&letter, tempBuffer+12+(i*2), 1);
						name.Append(letter);
					}

					//wxLogMessage(_T("ID: %i, Object File: %s,  Flag: 0x%X"), val, thisname.c_str(), flag);
					fileTree->AppendItem(root, name, -1, -1, new FileTreeData(val));    
					name = "";
				}

			} 
			else if (fileType == CACHE_SKELETON) 
			{
				wxString thisname; 
				unsigned char *tempBuffer = NULL;  
				unsigned char letter;  
				unsigned int size; 

				for (unsigned int t=0; t<fileCount; t++)
				{ 
					int val = cArchive.getFileID(t);
					tempBuffer = cArchive.getFile(t); 
					memcpy(&size, tempBuffer, 4);  

					for(unsigned int i = 0; i<size; i++) 
					{
						memcpy(&letter, tempBuffer+4+(i*2), 1);
						thisname.Append(letter);
					}

					fileTree->AppendItem(root, thisname, -1, -1, new FileTreeData(val));    
					thisname = "";
				}
			} 
			else if (fileType == CACHE_ZONE)
			{
				wxString name; 
				unsigned char *tempBuffer = NULL;  
				wxChar letter = 0;  
				int nameLength = 0; 

				for (unsigned int t=0; t<fileCount; t++)
				{ 
					int val = cArchive.getFileID(t);
					tempBuffer = cArchive.getFile(t); 
					memcpy(&nameLength, tempBuffer+4, 4);  

					for(int i=0; i<nameLength; i++)  
					{
						memcpy(&letter, tempBuffer+8+(i*2), sizeof(wxChar));
						name.Append(letter);
					}

					fileTree->AppendItem(root, name, -1, -1, new FileTreeData(val));    
					name = "";
					tempBuffer = 0;
				}
			} 
			else 
			{
				for (unsigned int i=0; i<fileCount; i++) 
				{
					unsigned int val = cArchive.getFileID(i);
					fileTree->AppendItem(root, wxString::Format(_T("%u"), val), -1, -1, new FileTreeData(val));
				}
			}
		}
		// Save open file
	} 
	else if (event.GetId() == ID_FILE_SAVE) 
	{
		if (fileType == CACHE_TEXTURE && canvas->tex) 
		{
			wxFileDialog dialog(this, _T("Save Texture"), wxEmptyString, 
				wxEmptyString, _T("Bitmap Image(*.bmp)|*.bmp"), wxSAVE|wxOVERWRITE_PROMPT);
			if (dialog.ShowModal()==wxID_OK) 
			{
				canvas->tex->SaveToFile(dialog.GetPath().c_str());
			}
		} 

		else 
		{
			wxFileDialog dialog(this, _T("Save File"), wxEmptyString, wxEmptyString,
				_T("Data (*.dat)|*.dat"), wxSAVE|wxOVERWRITE_PROMPT);
			if (dialog.ShowModal()==wxID_OK)
			{
				wxTreeItemId item = fileTree->GetSelection();
				FileTreeData *data = (FileTreeData *) fileTree->GetItemData(item);
				SaveFile(dialog.GetPath().c_str(), data->id);
			}
		}

		// open all known cache files
	} 
	else if (event.GetId() == ID_FILE_OPENALL) 
	{
		wxDirDialog dialog(this, _T("Select Cache folder"), _T("C:\\"), wxDEFAULT_DIALOG_STYLE);
		if (dialog.ShowModal()==wxID_OK) 
		{
			// Error check
			if (canvas) 
			{
				// Clear old
				wxDELETE(canvas->tex);
				wxDELETE(canvas->model);
				wxDELETE(canvas->skeleton);
				wxDELETE(canvas->obj);
			}

			// if the archives were already open, close them
			texArchive.close();
			meshArchive.close();
			objArchive.close();
			renderArchive.close();
			skeletonArchive.close();
			motionArchive.close();
			zoneArchive.close();

			wxString fn = dialog.GetPath();
			wxString file = fn + "\\textures.cache";
			
			// Open Textures archive
			texArchive.open(file.c_str());

			// Open Mesh archive
			file = fn + "\\mesh.cache";
			meshArchive.open(file.c_str());

			// Open Objects archive
			file = fn + "\\cobjects.cache";
			objArchive.open(file.c_str());

			// Open Render archive
			file = fn + "\\render.cache";
			renderArchive.open(file.c_str());

			// Open Skeleton archive
			file = fn + "\\skeleton.cache";
			skeletonArchive.open(file.c_str());

			// Open motion/animation archive
			file = fn + "\\motion.cache";
			motionArchive.open(file.c_str());

			// Open zone archive
			file = fn + "\\czone.cache";
			zoneArchive.open(file.c_str());

			fileType = CACHE_ALL;

			// Clear existing tree data
			fileTree->Unselect();
			fileTree->DeleteAllItems();

			// 0x01 = Sun
			// 0x03 = Basic objects,  Pillars, rocks, trees, monuments, static structures
			// 0x4 & 0x5 = Buildings and Interactive objects - don't know exact difference between 4 and 5.
			// 0x9 = Items, all weapons, armours, equipment, etc.
			// 0xD = All Runes / Creatures / NPCs / Characters
			// 0xF = All Deeds
			// 0x10 = Keys, Warrants, etc
			// 0x13 = Particles, Environment, Interface

			wxTreeItemId root = fileTree->AddRoot(_T("Root"));
			wxTreeItemId item = fileTree->AppendItem(root, _T("Items"));
			wxTreeItemId object = fileTree->AppendItem(root, _T("Props"));
			wxTreeItemId building = fileTree->AppendItem(root, _T("Structures"));
			wxTreeItemId creature = fileTree->AppendItem(root, _T("Runes & NPCs"));
			wxTreeItemId deed = fileTree->AppendItem(root, _T("Deeds"));
			wxTreeItemId particle = fileTree->AppendItem(root, _T("Effects"));
			wxTreeItemId other = fileTree->AppendItem(root, _T("Other"));
			wxTreeItemId zone = fileTree->AppendItem(root, _T("Zones"));


			wxString name; 
			unsigned char *tempBuffer = NULL;  
			unsigned char letter = 0;  
			unsigned int size = 0;
			unsigned int flag = 0;
			unsigned int fileCount = objArchive.getFileCount();
			wxTreeItemId node;

			// Load objects into our list
			for (unsigned int t=0; t<fileCount; t++) { 
				int val = objArchive.getFileID(t);
				tempBuffer = objArchive.getFile(t); 

				// Error check
				if (!tempBuffer)
					break;

				memcpy(&flag, tempBuffer + 4, 4);  
				memcpy(&size, tempBuffer + 8, 4);  

				for(unsigned int i = 0; i<size; i++)  {
					memcpy(&letter, tempBuffer+12+(i*2), sizeof(wxChar));
					name.Append(letter);
				}

				name.Append(wxString::Format(_T(" (%i)"), val));

				switch (flag) {
					case 3:
						node = object;
						break;
					case 4:
						node = building;
						break;
					case 5:
						node = building;
						break;
					case 9:
						node = item;
						break;
					case 13:
						node = creature;
						break;
					case 15:
						node = deed;
						break;
					case 19:
						node = particle;
						break;
					default:
						node = other;
				}

				fileTree->AppendItem(node, name, -1, -1, new FileTreeData(val));    
				name = "";
			}

			fileCount = zoneArchive.getFileCount();

			// Load our zones into our list
			for (unsigned int t=0; t<fileCount; t++) { 
				int val = zoneArchive.getFileID(t);
				tempBuffer = zoneArchive.getFile(t); 

				// Error check
				if (!tempBuffer)
					break;

				memcpy(&size, tempBuffer + 4, 4);  

				for(unsigned int i = 0; i<size; i++)  {
					memcpy(&letter, tempBuffer+8+(i*2), sizeof(wxChar));
					name.Append(letter);
				}

				name.Append(wxString::Format(_T(" (%i)"), val));

				fileTree->AppendItem(zone, name, -1, -1, new FileTreeData(val, true));    
				name = "";
			}
		}
	}

}

// view menu commands
void SBViewer::OnViewCommand(wxCommandEvent &event)
{
	if (event.GetId()==ID_VIEW_WIREFRAME) {
		canvas->renderMode = GL_LINE;

	} else if (event.GetId()==ID_VIEW_SOLID) {
		canvas->renderMode = GL_FILL;

	} else if (event.GetId()==ID_VIEW_LIGHT) {
		if (event.IsChecked())
			canvas->InitLighting();
		else
			glDisable(GL_LIGHTING);

	} else if (event.GetId()==ID_VIEW_BOUNDS) {
		canvas->drawBounds = event.IsChecked();

	} else if (event.GetId()==ID_VIEW_WALKABLE) {
		canvas->drawWalkable = event.IsChecked();

	} else if (event.GetId()==ID_VIEW_SKELETON) {
		canvas->drawSkeleton = event.IsChecked();

	} else if (event.GetId()==ID_GROUND_DRAW) {
		canvas->drawGround = event.IsChecked();

	} else if (event.GetId()== ID_GROUND_GRASS) {
		canvas->LoadGround(360);

	} else if (event.GetId()==ID_GROUND_DESERT) {
		canvas->LoadGround(370);

	} else if (event.GetId()==ID_GROUND_UNDEAD) {
		canvas->LoadGround(5050100);

	} else if (event.GetId()==ID_GROUND_VOLCANIC) {
		canvas->LoadGround(350);

	} else if (event.GetId()==ID_GROUND_SNOW) {
		canvas->LoadGround(0);

	} else if (event.GetId()==ID_GROUND_CHECKER) {
		canvas->LoadGround(0);

	}

}

void SBViewer::OnTreeSelect(wxTreeEvent &event)
{
	// Creates and pushes an hourglass cursor on the stack
	wxBusyCursor wait;

	wxTreeItemId item = event.GetItem();

	// Error check
	if (!item.IsOk()) // make sure that a valid Tree Item was actually selected.
	{
		return;
	}

	unsigned char *bufPtr = NULL;
	unsigned int fileSize = 0;

	FileTreeData *data = (FileTreeData *) fileTree->GetItemData(item);

	if (!data)
	{
		return;
	}

	// Debug info
	wxLogMessage(_T("Opening File ID: %i"), data->id);

	if (fileType != CACHE_ALL) 
	{
		// set our vars with the data from cache
		bufPtr = cArchive.getFileByID(data->id);
		fileSize = cArchive.getFileSizeByID(data->id);

		// Error check
		if (!bufPtr)
		{
			return;
		}

		// textures.cache
		if (fileType == CACHE_TEXTURE)
		{
			wxDELETE(canvas->tex);
			canvas->tex = new Texture(bufPtr, fileSize);
			wxString name;

			name << wxT("E:\\Shadowbane Stuff\\Images\\") << data->id <<wxT(".bmp");

			canvas->tex->SaveToFile(name.c_str());
			// mesh.cache
		}
		else if (fileType == CACHE_MESH)
		{
			wxDELETE(canvas->model);
			canvas->model = new Model(bufPtr);
		}

		// skeleton.cache
		else if (fileType == CACHE_SKELETON)
		{
			if (bufPtr) 
			{
				wxDELETE(canvas->skeleton);
				canvas->skeleton = new Skeleton(bufPtr, fileSize);
				animControl->UpdateSkeleton(canvas->skeleton);
			}

		}
		// sounds.cache
		else if (fileType == CACHE_SOUND) 
		{
			//unsigned int mode = FSOUND_LOADRAW|FSOUND_LOADMEMORY|FSOUND_LOOP_OFF|FSOUND_16BITS;;

			//int bitrate;
			//int freq;
			//memcpy(&freq, bufPtr+4, 4);
			//memcpy(&bitrate, bufPtr+12, 4);

			////if (bitrate == 16)
			////	mode |= FSOUND_16BITS;
			////else
			////	mode |= FSOUND_8BITS;

			//FSOUND_SAMPLE *soundFile = FSOUND_Sample_Load(0, (char*)bufPtr, mode, 16, fileSize);
			//if (soundFile) {
			//	FSOUND_Sample_SetDefaults(soundFile, freq, -1, FSOUND_STEREO, -1);
			//	FSOUND_PlaySound(0, soundFile);
			//}


			// czone.cache
		} 
		else if (fileType == CACHE_ZONE)
		{
		}


		// We have Opened all needed cache files to display complete objects
	} else if (fileType == CACHE_ALL) {

		// Load zone
		if (data->zone)
		{
			wxDELETE(canvas->zone);
			canvas->zone = new CZone(data->id);

			// Load Object
		} 
		else 
		{
			canvas->LoadObject(data->id);

			if (canvas->obj && canvas->obj->GetSkeleton())
			{
				animControl->UpdateSkeleton(canvas->obj->GetSkeleton());
			}
		}
	}

}

void SBViewer::SaveFile(const wxChar *fn, unsigned int fileIndex)
{
	FILE *file = NULL;

	// open the file as write/binary
#ifdef _UNICODE
	file = _wfopen(fn, _T("wb+"));
#else
	file = fopen(fn, _T("wb+"));
#endif

	unsigned char *buffer = 0;
	unsigned int size = 0;

	if (fileType == CACHE_ALL)
	{
		buffer = objArchive.getFileByID(fileIndex);
		size = objArchive.getFileSizeByID(fileIndex);
	} 
	else
	{
		buffer = cArchive.getFileByID(fileIndex);
		size = cArchive.getFileSizeByID(fileIndex);
	}

	// error check
	if (file) 
	{
		// error check
		if (buffer)
		{
			fwrite(buffer, 1, size, file);
		}

		// close the file
		fclose(file);
	}
}


SBViewer::~SBViewer()
{
	// Fmod
	// FMOD_RESULT close();

	// wxAUI stuff
	interfaceManager.UnInit();

	// close the *.cache files
	cArchive.close();

	texArchive.close();
	meshArchive.close();
	objArchive.close();
	renderArchive.close();
	skeletonArchive.close();
	motionArchive.close();
}

