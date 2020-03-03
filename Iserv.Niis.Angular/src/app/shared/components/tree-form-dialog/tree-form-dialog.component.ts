import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TreeNode } from 'primeng/components/common/treenode';

interface IExpandedNode {
  id: number;
  label: string;
}

export interface IOrderedSelectedNode {
  id: number;
  index: number;
  label?: string;
}

@Component({
  selector: 'app-tree-form-dialog',
  templateUrl: './tree-form-dialog.component.html',
  styleUrls: ['./tree-form-dialog.component.scss']
})
export class TreeFormDialogComponent implements OnInit {
  formGroup: FormGroup;
  tree: TreeNode[] = [];
  filteredValues: TreeNode[] = [];
  selectedNodes: TreeNode[] = [];
  expandedNodes: IExpandedNode[] = [];
  orderedSelectedNodes: IOrderedSelectedNode[] = [];

  constructor(
    private dialogRef: MatDialogRef<TreeFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.buildForm();
  }

  ngOnInit(): void {
    const filter = (value, root) => {
      return root.filter((entry) => {
        if (entry.children && entry.children.length) {
          const result = filter(value, entry.children);
          if (result.length) {
            entry.children = result;

            return result;
          }
        }

        return entry.label.toLowerCase().includes(value.toLowerCase());
      });
    };

    const expand = (data, needDeep = true) => {
      for (let entry of data) {
        if (entry.children) {
          entry.expanded = true;

          if (needDeep) {
            expand(entry.children);
          }
        }
      }
    };

    const copy = (data) => {
      try {
        return JSON.parse(JSON.stringify(data));
      } catch (error) {
        console.error(error);

        return data.slice();
      }
    };

    this.filteredValues = copy(this.data.tree);
    if (this.filteredValues.length === 1) {
      expand(this.filteredValues, false);
    }
    let previousValue = null;

    this.setSelectedNodes();
    this.formGroup.get('filter').valueChanges
      .subscribe(() => {
      const value = this.formGroup.get('filter').value;
        if (value) {
          this.filteredValues = filter(value, copy(this.data.tree));

          if (value.length >= 3) {
            expand(this.filteredValues);
          }
        }

        if (previousValue && previousValue.length > value.length) {
          this.filteredValues = filter(value, copy(this.data.tree));
        }

        const selectedIDs = Array.from(new Set(this.selectedNodes.map((entry) => (entry.data))));
        for (let index = 0; index < this.filteredValues.length; index++) {
          this.setSelectedNode(this.filteredValues[index], selectedIDs);
        }

        previousValue = value;
      });
  }

  onReset(): void {
    this.formGroup.get('filter').setValue('');
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onSubmit(): void {
    this.orderedSelectedNodes = [];
    for (let selectedNode of this.selectedNodes) {
      if (!selectedNode.children.length) {
        this.orderedSelectedNodes.push({
          id: selectedNode.data,
          index: this.orderedSelectedNodes.length,
          label: selectedNode.label
        });
      }
    }
    this.orderedSelectedNodes.sort((a, b) => (a.index - b.index));

    this.dialogRef.close(this.orderedSelectedNodes);
  }

  private setSelectedNode(treeNode: TreeNode, ids: number[], parents: TreeNode[] = []): void {
    if (ids.includes(treeNode.data)) {
      treeNode.leaf = true;

      if (treeNode.data > 0) {
        this.selectedNodes.forEach((entry, index) => {
          if (entry.data === treeNode.data && entry.label === treeNode.label) {
            this.selectedNodes.splice(index, 1);
          }
        });

        this.selectedNodes.push(treeNode);
      }

      if (!this.expandedNodes.find((entry) => (entry.id === treeNode.data && entry.label === treeNode.label))) {
        this.expandedNodes.push({
          id: treeNode.data,
          label: treeNode.label
        });
      }

      if (parents.length) {
        for (let parent of parents) {
          if (this.expandedNodes.find((entry) => (entry.id === treeNode.data && entry.label === treeNode.label))) {
            parent.partialSelected = true;
            parent.expanded = true;
          } else {
            parent.partialSelected = false;
            parent.expanded = false;
          }
        }
      }
    }

    if (treeNode.children) {
      for (let child of treeNode.children) {
        this.setSelectedNode(child, ids, [...parents, treeNode]);
      }
    }
  }

  private setSelectedNodes(): void {
    if (this.data.ids) {
      for (let id of this.data.ids) {
        this.orderedSelectedNodes.push({
          id,
          index: this.orderedSelectedNodes.length
        });
      }
      for (let index = 0; index < this.filteredValues.length; index++) {
        this.setSelectedNode(this.filteredValues[index], this.data.ids);
      }
    }
  }

  private buildForm(): void {
    this.formGroup = new FormGroup({
      filter: new FormControl()
    });
  }
}
