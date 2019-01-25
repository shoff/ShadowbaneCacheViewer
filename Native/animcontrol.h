
#pragma once

#include "stdafx.h"
#include "skeleton.h"
#include "enums.h"

class AnimControl: public wxWindow
{
	DECLARE_CLASS(AnimControl)
    DECLARE_EVENT_TABLE()

	wxComboBox *animList;

	unsigned int selectedAnim;

	Skeleton *skeleton;

public:
	AnimControl(wxWindow* parent, wxWindowID id);
	~AnimControl();

	void OnAnim(wxCommandEvent &event);

	void UpdateSkeleton(Skeleton *s);
};



// --