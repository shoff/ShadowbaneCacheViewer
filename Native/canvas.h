
#pragma once

#include "stdafx.h"
#include "texture.h"
#include "model.h"
#include "skeleton.h"
#include "objects.h"
#include "zones.h"
#include "wx/wx.h"
#include "wx/glcanvas.h"


class Canvas: public wxGLCanvas
{
private:
	DECLARE_CLASS(Canvas)
    DECLARE_EVENT_TABLE()

	Vec3D vRot, vRot0;	// Model Rotation
	Vec3D vPos, vPos0;	// Model Position
	wxCoord mx, my;		// Mouse coords
	
public:
	Canvas(wxWindow *parent, wxWindowID id = wxID_ANY,
        const wxPoint& pos = wxDefaultPosition,
        const wxSize& size = wxDefaultSize, long style = 0,
        const wxString& name = _T("Canvas"), int *attribList = 0,
        const wxPalette& palette = wxNullPalette, const int pixelFormat = 0);

    ~Canvas();
	
	// Event Handlers
    void OnPaint(wxPaintEvent& WXUNUSED(event));
    void OnSize(wxSizeEvent& event);
    void OnMouse(wxMouseEvent& event);
	void OnKey(wxKeyEvent &event);
    void OnTimer(wxTimerEvent& event);

	//void tick();
	void Zoom(float f, bool rel);

	// OGL related functions
	void InitGL();
	void InitView();
	void Render();
	void InitLighting();
	static void DrawGround();

	// Other funcs
	void LoadObject(unsigned int id);
	void LoadGround(unsigned int id);

	wxTimer timer;
	GLenum renderMode;

	// Objs
	Texture *tex;
	Model *model;
	Skeleton *skeleton;
	CObject *obj;
	CObject *ground;
	CZone *zone;

	// flags
	bool drawBounds;
	bool drawWalkable;
	bool drawSkeleton;
	bool drawGround;
};

