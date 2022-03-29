using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.DependencyResolvers.Ninject;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //_productService = new ProductManager(new EfProductDal());
            _productService = InstanceFactory.GetInstance<IProductService>();
            //_categoryService = new CategoryManager(new EfCategoryDal());
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();
            InitializeComponent();
        }

        private IProductService _productService;
        private ICategoryService _categoryService;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProduct();
            LoadCategory();
        }

        private void LoadProduct()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void LoadCategory()
        {
            cbxCategoryAdd.DataSource = _categoryService.GetAll();
            cbxCategoryAdd.DisplayMember = "CategoryName";
            cbxCategoryAdd.ValueMember = "CategoryID";

            cbxCategoryUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryUpdate.DisplayMember = "CategoryName";
            cbxCategoryUpdate.ValueMember = "CategoryID";

            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryID";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _productService.Add(new Product
            {
                CategoryID = Convert.ToInt32(cbxCategoryAdd.SelectedValue),
                ProductName = tbxProductNameAdd.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPriceAdd.Text),
                QuantityPerUnit = tbxQuantityPerUnitAdd.Text,
                UnitsInStock = Convert.ToInt16(tbxUnitsInStockAdd.Text)
            });
            MessageBox.Show("Added!");
            LoadProduct();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _productService.Delete(new Product
            {
                ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value)
            });
            MessageBox.Show("Deleted!");
            LoadProduct();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _productService.Update(new Product
            {
                ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                CategoryID = Convert.ToInt32(cbxCategoryUpdate.SelectedValue),
                ProductName = tbxProductNameUpdate.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),
                QuantityPerUnit = tbxQuantityPerUnitUpdate.Text,
                UnitsInStock = Convert.ToInt16(tbxUnitsInStockUpdate.Text)
            });
            MessageBox.Show("Updated!");
            LoadProduct();
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbxCategoryUpdate.SelectedValue = dgwProduct.CurrentRow.Cells[1].Value;
            tbxProductNameUpdate.Text = dgwProduct.CurrentRow.Cells[2].Value.ToString();
            tbxUnitPriceUpdate.Text = Convert.ToDecimal(dgwProduct.CurrentRow.Cells[3].Value).ToString();
            tbxQuantityPerUnitUpdate.Text = dgwProduct.CurrentRow.Cells[4].Value.ToString();
            tbxUnitsInStockUpdate.Text = Convert.ToInt16(dgwProduct.CurrentRow.Cells[5].Value).ToString();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch
            {
            }
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxSearch.Text))
            {
                dgwProduct.DataSource = _productService.GetProductsByProduct(tbxSearch.Text);
            }
            else
            {
                LoadProduct();
            }
        }
    }
}
