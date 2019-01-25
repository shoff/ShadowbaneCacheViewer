
#include "animcontrol.h"


IMPLEMENT_CLASS(AnimControl, wxWindow)

BEGIN_EVENT_TABLE(AnimControl, wxWindow)
	EVT_COMBOBOX(ID_ANIM_LIST, AnimControl::OnAnim)
END_EVENT_TABLE()

AnimControl::AnimControl(wxWindow* parent, wxWindowID id)
{
	wxLogMessage(_T("Creating Anim Control..."));

	skeleton = 0;

	if(Create(parent, id, wxDefaultPosition, wxSize(700,90), 0, _T("AnimControlFrame")) == false) {
		wxLogMessage(_T("GUI Error: Failed to create a window for our AnimControl!"));
		return;
	}

	animList = new wxComboBox(this, ID_ANIM_LIST, _T("Animation"), wxPoint(10,10), wxSize(144,16), 0, NULL, wxCB_READONLY, wxDefaultValidator, _T("Animation")); //|wxCB_SORT
}

AnimControl::~AnimControl()
{
	animList->Clear();

	animList->Destroy();
}


void AnimControl::OnAnim(wxCommandEvent &event)
{
	if (!skeleton)
		return;

	if (event.GetId() == ID_ANIM_LIST) {
		wxString val = animList->GetValue();

		// Selection anim (motion) file id
		selectedAnim = atoi(val.c_str());
		
		// Error range check
		if (selectedAnim > 1000000 && selectedAnim < 99999999)
			skeleton->SetAnimation(selectedAnim);
	}
}

void AnimControl::UpdateSkeleton(Skeleton *s) 
{
	skeleton = s;

	animList->Clear();

	if (!skeleton)
		return;

	for (size_t i=0; i<skeleton->animations.size(); i++) {
		animList->Append(wxString::Format("%i", skeleton->animations[i]));
	}

	animList->SetSelection(0);
}


// --