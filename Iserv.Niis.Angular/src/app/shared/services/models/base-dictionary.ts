import { User } from '../../../administration/components/users/models/user.model';
import { SelectOption } from './select-option';
import { TreeNode } from 'primeng/components/common/treenode';

export interface BaseDictionary extends SelectOption {
    dateCreate: Date;
    nameKz: string;
    nameEn: string;
    description: string;
    descriptionField?: string;

}

export interface DicRouteStage extends BaseDictionary {
    isSystem: boolean;
    interval: number;
    isFirst: boolean;
    isLast: boolean;
    isAuto: boolean;
    isMain: boolean;
    isMultiUser: boolean;
    isReturnable: boolean;
    onlineRequisitionStatusId: number;
    routeId: number;
}

export interface DicReceiveType extends BaseDictionary {
    receiveTypeId: number;
}

export interface DicTariff extends BaseDictionary {
    receiveTypeId: number;
}

export interface DicProtectionDocType extends BaseDictionary {
    routeId: number;
}

export interface DicContractType extends BaseDictionary {
  stageOneId: number;
  stageTwoId: number;
}

export interface DicProtectionDocSubType extends BaseDictionary {
    typeId: number;
}

export interface DicDocumentType extends BaseDictionary {
    classificationId: number;
}

export interface DicDivision extends BaseDictionary {
    divisionId: number;
}

export interface DicDepartment extends BaseDictionary {
    divisionId: number;
    departmentTypeId: number;
    isMonitoring: boolean;
    tNameRu: string;
    shortNameRu: string;
}

export interface DicPosition extends BaseDictionary {
    isMonitoring: boolean;
    isHead: boolean;
    departmentId: number;
    positionTypeId: number;
    positionTypeCode: string;
}

export interface Classification extends BaseDictionary {
    parentId: number;
}

export interface ExtendedTreeNode extends TreeNode {
    parentId: number;
    isFinal: boolean;
}

export function toTreeNodes(classification: Classification[], icon = 'fa-folder', rootIcon = 'fa-folder', finalIcon = 'fa-folder')
    : ExtendedTreeNode[] {
    const treeList: ExtendedTreeNode[] = [];
    const lookup: ExtendedTreeNode[] = [];

    classification
        .map((element: Classification): ExtendedTreeNode => {
            const node = {
                parentId: element.parentId,
                data: element.id,
                label: element.nameRu,
                collapsedIcon: icon,
                expandedIcon: `${icon}-o`,
                children: [],
                // leaf: boolean,
                // expanded: boolean,
                // type: string,
                // parent: TreeNode,
                // partialSelected: boolean,
                // styleClass: string,
                // draggable: boolean,
                // droppable: boolean,
                // selectable: boolean,
            } as ExtendedTreeNode;

            lookup[node.data] = node;
            return node;
        })
        .forEach(treeNode => {
            if (treeNode.parentId != null) {
                lookup[treeNode.parentId].children.push(treeNode);

                if (!lookup.map(n => n.parentId).includes(treeNode.data)) {
                    treeNode.isFinal = true;
                    treeNode.collapsedIcon = finalIcon;
                    treeNode.expandedIcon = `${finalIcon}-o`;
                }
            } else {
                treeNode.collapsedIcon = rootIcon;
                treeNode.expandedIcon = `${rootIcon}-o`;
                treeList.push(treeNode);
            }
        });
    return treeList;
}

export function toDocumentTypeTreeNodes(classification: Classification[], documentTypes: DicDocumentType[], icon = 'fa-folder',
    rootIcon = 'fa-folder', finalIcon = 'fa-file-o'): ExtendedTreeNode[] {
    const treeList: ExtendedTreeNode[] = [];
    const filteredList: ExtendedTreeNode[] = [];
    const lookup: ExtendedTreeNode[] = [];
    const hiddenNodeLabels: string[] = ['Заявки', 'Заявление на Коммерциализацию'];

    const concatedData = documentTypes
        .map((element: DicDocumentType): ExtendedTreeNode => {
            const node = {
                parentId: element.classificationId,
                data: element.id,
                label: element.nameRu,
                isFinal: true && !!element.classificationId,
                collapsedIcon: finalIcon,
                expandedIcon: `${finalIcon}-o`,
                children: [],
            } as ExtendedTreeNode;

            lookup[node.data] = node;
            return node;
        })
        .concat(
        classification
            .map((element: Classification): ExtendedTreeNode => {
                const node = {
                    parentId: element.parentId,
                    data: element.id,
                    label: element.nameRu,
                    collapsedIcon: icon,
                    expandedIcon: `${icon}-o`,
                    children: [],
                } as ExtendedTreeNode;

                lookup[node.data] = node;
                return node;
            })
        );

    concatedData.forEach(treeNode => {
        if (treeNode.parentId != null) {
            if (!hiddenNodeLabels.includes(treeNode.label)) {
                lookup[treeNode.parentId].children.push(treeNode);
            }
        } else {
            treeNode.collapsedIcon = rootIcon;
            treeNode.expandedIcon = `${rootIcon}-o`;
            treeList.push(treeNode);
        }
    });

    removeEmptyNodes(treeList);
    sort(treeList, true);

    return treeList;
}

export function toUserTreeNodes(divisions: SelectOption[], departments: DicDepartment[], users: User[], icon = 'fa-users',
    rootIcon = 'fa-building', finalIcon = 'fa-user-o'): ExtendedTreeNode[] {
    const treeList: ExtendedTreeNode[] = [];
    const filteredList: ExtendedTreeNode[] = [];
    const lookup: ExtendedTreeNode[] = [];

    const concatedData = users
        .map((element: User): ExtendedTreeNode => {
            const node = {
                parentId: element.departmentId,
                data: element.id,
                label: element.nameRu,
                isFinal: true,
                collapsedIcon: finalIcon,
                expandedIcon: `${finalIcon}-o`,
                children: [],
            } as ExtendedTreeNode;

            lookup[node.data] = node;
            return node;
        })
        .concat(
        departments
            .map((element: DicDepartment): ExtendedTreeNode => {
                const node = {
                    parentId: element.divisionId,
                    data: element.id,
                    label: element.nameRu,
                    collapsedIcon: icon,
                    expandedIcon: `${icon}`,
                    children: [],
                } as ExtendedTreeNode;

                lookup[node.data] = node;
                return node;
            })
            .concat(
            divisions
                .map((element: SelectOption): ExtendedTreeNode => {
                    const node = {
                        data: element.id,
                        label: element.nameRu,
                        collapsedIcon: icon,
                        expandedIcon: `${icon}-o`,
                        children: [],
                    } as ExtendedTreeNode;

                    lookup[node.data] = node;
                    return node;
                })
            ));


    concatedData.forEach(treeNode => {
        if (treeNode.parentId != null) {
            lookup[treeNode.parentId].children.push(treeNode);
        } else {
            treeNode.collapsedIcon = rootIcon;
            treeNode.expandedIcon = `${rootIcon}-o`;
            treeList.push(treeNode);
        }
    });

    // removeEmptyNodes(treeList);
    sort(treeList);

    return treeList;
}

function removeEmptyNodes(nodes: TreeNode[]): void {
    for (let i = 0; i < nodes.length; i++) {
        const node = nodes[i];
        removeEmptyNodes(node.children);

        if (node.children.length === 0 && !((node as ExtendedTreeNode).isFinal)) {
            const index = nodes.indexOf(node);
            if (index > -1) {
                nodes.splice(index, 1);
                i--;
            }
        }
    }
}

function sort(nodes: TreeNode[], skipRoot = false): void {
    for (let i = 0; i < nodes.length; i++) {
        const node = nodes[i];
        sort(node.children);
    }

    nodes.sort((n1: ExtendedTreeNode, n2: ExtendedTreeNode) => {
        if (skipRoot && !(n1.parentId || n2.parentId)) { return 0; }
        if (!n1.isFinal > !n2.isFinal) {
            return -1;
        } else if (!n1.isFinal < !n2.isFinal) {
            return 1;
        }

        if (n1.label < n2.label) {
            return -1;
        } else if (n1.label > n2.label) {
            return 1;
        } else {
            return 0;
        }
    });
}
