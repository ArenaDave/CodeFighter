using CodeFighter.Data;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HullMaker
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<Keel> allKeels = Keel.All();
                List<Tuple<string, string>> keelSource = new List<Tuple<string, string>>();
                keelSource.Add(new Tuple<string, string>("All", ""));
                foreach (Keel k in allKeels)
                    keelSource.Add(new Tuple<string, string>(k.Name, k.Designator));
                
                ddlKeel.DataSource = keelSource;
                ddlKeel.DataTextField = "Item1";
                ddlKeel.DataValueField = "Item2";
                ddlKeel.DataBind();

                List<ShipHull> hulls = DataFactory.GetShipHulls(string.Empty);
                gvHulls.DataSource = hulls;
                gvHulls.DataBind();
            }
        }


        protected void gvHulls_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;
            if(row.RowType == DataControlRowType.DataRow)
            {
                ShipHull hull = (ShipHull)row.DataItem;
                LinkButton lbnEditHull = (LinkButton)row.FindControl("lbnEditHull");
                lbnEditHull.CommandArgument = hull.Id.ToString();
                Literal litKeelName = (Literal)row.FindControl("litKeelName");
                litKeelName.Text = hull.Size.Name;
                Literal litHitpoints = (Literal)row.FindControl("litHitpoints");
                litHitpoints.Text = hull.Hitpoints.ToString();
                Literal litWeapons = (Literal)row.FindControl("litWeapons");
                int beams = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(WeaponPart)) && x.ActionMechanism == "Beam");
                int cannons = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(WeaponPart)) && x.ActionMechanism == "Cannon");
                int launchers = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(WeaponPart)) && x.ActionMechanism == "Launcher");
                litWeapons.Text = string.Format("B{0}/C{1}/L{2}", beams, cannons, launchers);
                Literal litDefenses = (Literal)row.FindControl("litDefenses");
                int shields = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(DefensePart)) && x.ActionMechanism == "Shield");
                int armors = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(DefensePart)) && x.ActionMechanism == "Armor");
                int pds = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(DefensePart)) && x.ActionMechanism == "PointDefense");
                litDefenses.Text = string.Format("S{0}/A{1}/PD{2}", shields, armors, pds);
                Literal litActions = (Literal)row.FindControl("litActions");
                litActions.Text = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(ActionPart))).ToString();
                Literal litEngines = (Literal)row.FindControl("litEngines");
                litEngines.Text = hull.MaxPartsByType.Count(x => x.PartType.Equals(typeof(EnginePart))).ToString();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("HullDetail.aspx?shipHullId=-1");
        }

        protected void lbnEditHull_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect(string.Format("HullDetail.aspx?shipHullId={0}", e.CommandArgument));
        }

        protected void ddlKeel_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ShipHull> hulls = DataFactory.GetShipHulls(ddlKeel.SelectedValue);
            gvHulls.DataSource = hulls;
            gvHulls.DataBind();
        }
    }
}